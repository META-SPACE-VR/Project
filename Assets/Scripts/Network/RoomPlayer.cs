using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

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
    [Networked] public NetworkBool IsReady { get; set; }
    [Networked] public NetworkString<_16> Username { get; set; }
    [Networked] public NetworkBool HasFinished { get; set; }
    [Networked] public EGameState GameState { get; set; }
    [Networked] public NetworkObject PlayerObject { get; set; }

    public static Action<RoomPlayer> PlayerJoined;
    public static Action<RoomPlayer> PlayerLeft;
    public static Action<RoomPlayer> PlayerChanged;
    
    public static RoomPlayer Local;

    
    [Networked, OnChangedRender(nameof(NicknameChanged))]
    public NetworkString<_16> Nickname { get; set; }
    [Networked] public byte ColorIndex { get; set; } // Job Index
    // [Networked, OnChangedRender(nameof(ColorChanged))]
	// public byte ColorIndex { get; set; } //Job Index
	public Color GetColor => GameManager.rm.playerColours[ColorIndex];

    public bool IsLeader => Object!=null && Object.IsValid && Object.HasStateAuthority;

    private ChangeDetector _changeDetector;

    public void Server_Init(PlayerRef pRef, byte index, byte color) 
    {
        Debug.Assert(Runner.IsServer);
        Ref = pRef;
        Index = index;
        ColorIndex = color;
    }

    public override void Spawned()
    {
        base.Spawned();

        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

		// if (Object.HasStateAuthority)
		// {
        //     PlayerRegistry.Server_Add(Runner, Object.InputAuthority, this);
		// }

        if (Object.HasInputAuthority)
        {
            Local = this;
            PlayerChanged?.Invoke(this);
            RPC_SetPlayerStats(ClientInfo.Username);
            Rpc_SetNickname(PlayerPrefs.GetString("nickname"));
        }

        NicknameChanged();
        // ColorChanged();

        if (!Players.Contains(this))
        {
            Players.Add(this);
            PlayerJoined?.Invoke(this);
        }

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
    private void RPC_SetPlayerStats(NetworkString<_16> username)
    {
        Username = username;
    }

	[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
	void Rpc_SetNickname(string nick)
	{
		Nickname = nick;
	}

	// [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
	// public void Rpc_SetColor(byte c)
	// {
	// 	if (PlayerRegistry.IsColorAvailable(c))
	// 		ColorIndex = c;
	// }

	void NicknameChanged()
	{
		GetComponent<PlayerData>().SetNickname(Nickname.Value);
	}
	// void ColorChanged()
	// {
	// 	GetComponent<PlayerData>().SetColour(GetColor);
	// }


    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_ChangeReadyState(NetworkBool state)
    {
        Debug.Log($"Setting {Object.Name} ready state to {state}");
        IsReady = state;
    }

    private void OnDisable()
    {
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

        // PlayerRegistry 관련 코드는 주석 처리
        // RoomPlayer leftPlayer = PlayerRegistry.GetPlayer(p);
        // if (leftPlayer != null) runner.Despawn(leftPlayer.Object);
    }

    public bool IsSpawned()
    {
        return PlayerObject != null && PlayerObject.IsValid;
    }
}