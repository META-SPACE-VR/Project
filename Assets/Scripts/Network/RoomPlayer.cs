using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

// PlayerObject
public class RoomPlayer : NetworkBehaviour
{
    public enum EGameState
    {
        Lobby,
        GameCutscene,
        GameReady
    }

    public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();

	[Networked] public PlayerRef Ref { get; set; }
	[Networked] public byte Index { get; set; } 
	[Networked] public byte ColorIndex { get; set; } //Job Index
    public static Action<RoomPlayer> PlayerJoined;
    public static Action<RoomPlayer> PlayerLeft;
    public static Action<RoomPlayer> PlayerChanged;
    
    public static RoomPlayer Local;

    [Networked] public NetworkBool IsReady { get; set; }
	[Networked] public NetworkString<_32> Username { get; set; }
	[Networked] public NetworkBool HasFinished { get; set; }
    [Networked] public EGameState GameState { get; set; }

	// [field: SerializeField] public VoiceNetworkObject VoiceObject { get; private set; }


	public bool IsLeader => Object!=null && Object.IsValid && Object.HasStateAuthority;

	private ChangeDetector _changeDetector;

	public void Server_Init(PlayerRef pRef, byte index, byte color) {
		Debug.Assert(Runner.IsServer);
		Ref = pRef;
		Index = index;
		ColorIndex = color;
	}

    public override void Spawned()
    {
        base.Spawned();

		_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if (Object.HasInputAuthority)
		{
			Local = this;

			PlayerChanged?.Invoke(this);
			RPC_SetPlayerStats(ClientInfo.Username);
		}

		Players.Add(this);
		PlayerJoined?.Invoke(this);

		DontDestroyOnLoad(gameObject);
    }

   	public override void Render()
	{
 		foreach (var change in _changeDetector.DetectChanges(this))
		{
			switch (change)
			{
				case nameof(IsReady):
				case nameof(Username):
					OnStateChanged(this);
					break;
			}
		}
	}

	[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
	private void RPC_SetPlayerStats(NetworkString<_32> username)
	{
		Username = username;
	}

	[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
	public void RPC_ChangeReadyState(NetworkBool state)
	{
		Debug.Log($"Setting {Object.Name} ready state to {state}");
		IsReady = state;
	}

	private void OnDisable()
	{
		// OnDestroy does not get called for pooled objects
		PlayerLeft?.Invoke(this);
		Players.Remove(this);
	}

	private static void OnStateChanged(RoomPlayer changed) => PlayerChanged?.Invoke(changed);

	public static void RemovePlayer(NetworkRunner runner, PlayerRef p)
	{
		var roomPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == p);
		if (roomPlayer != null)
		{
			Players.Remove(roomPlayer);
			runner.Despawn(roomPlayer.Object);
		}

	// 필요한 코드인지 모르겠지만 일단 추가
	// todo: PlayerRegistry 사용할 거면 활성화해보기 
	RoomPlayer leftPlayer = PlayerRegistry.GetPlayer(p);
	if (leftPlayer != null) runner.Despawn(leftPlayer.Object);
	}

}
