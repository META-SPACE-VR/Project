using Fusion;
using UnityEngine;

public class Map : NetworkBehaviour
{
	public static Map Current { get; private set; }

	[Networked] public TickTimer StartRaceTimer { get; set; }

	// public Checkpoint[] checkpoints;
	public Transform[] spawnpoints;
	public string music = "";

    [SerializeField] private GameObject playerPrefab; 

	private void Awake()
	{
		Current = this;
		// InitCheckpoints();

		// Initialize cutscene
		AudioManager.StopMusic();
        GameManager.SetMap(this);
		AudioManager.PlayMusic("game");
        // GameManager.Instance.camera = Camera.main;
		// GameManager.GetCameraControl(this);
	}

	public override void Spawned()
	{
		base.Spawned();

		// if (RoomPlayer.Local.IsLeader)
		// {
		// 	StartRaceTimer = TickTimer.CreateFromSeconds(Runner, sequence.duration + 4f);
		// }
		// sequence.StartSequence();
	}

	private void OnDestroy()
	{
		// GameManager.SetTrack(null);
	}

	public void SpawnPlayer(NetworkRunner runner, RoomPlayer player)
	{
		
        Debug.Log("여기 들어왔니");
        var index = RoomPlayer.Players.IndexOf(player);
		var point = spawnpoints[index];

        var prefab = playerPrefab;
		// var prefabId = player.KartId;
		// var prefab = ResourceManager.Instance.kartDefinitions[prefabId].prefab;

		// Spawn player
		var entity = runner.Spawn(
			prefab,
			point.position,
			point.rotation,
			player.Object.InputAuthority
		);

		// entity.Controller.RoomUser = player;
		player.GameState = RoomPlayer.EGameState.GameCutscene;
		// player.Kart = entity.Controller;

		// Debug.Log($"Spawning kart for {player.Username} as {entity.name}");
		// entity.transform.name = $"Kart ({player.Username})";
	}

	// private void InitCheckpoints()
	// {
	// 	for (int i = 0; i < checkpoints.Length; i++)
	// 	{
	// 		checkpoints[i].index = i;
	// 	}
	// }
}