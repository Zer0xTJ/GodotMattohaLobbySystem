using Godot;
using Godot.Collections;
using Mattoha.Core.Demo;
using Mattoha.Core.Utils;

namespace Mattoha.Nodes;
public partial class MattohaClient : Node
{
	[Signal] public delegate void ConnectedToServerEventHandler();
	[Signal] public delegate void PlayerRegisteredEventHandler(Dictionary<string, Variant> playerData);
	[Signal] public delegate void LoadAvailableLobbiesSucceedEventHandler(Array<Dictionary<string, Variant>> lobbies);
	[Signal] public delegate void LoadLobbyPlayersSucceedEventHandler(Array<Dictionary<string, Variant>> players);

	[Signal] public delegate void SetPlayerDataSucceedEventHandler(Dictionary<string, Variant> playerData);
	[Signal] public delegate void SetPlayerDataFailedEventHandler(string cause);

	[Signal] public delegate void CreateLobbySucceedEventHandler(Dictionary<string, Variant> lobbyData);
	[Signal] public delegate void CreateLobbyFailedEventHandler(string cause);

	[Signal] public delegate void StartGameSucceedEventHandler(Dictionary<string, Variant> lobbyData);
	[Signal] public delegate void StartGameFailedEventHandler(string cause);

	[Signal] public delegate void JoinLobbySucceedEventHandler(Dictionary<string, Variant> lobbyData);
	[Signal] public delegate void JoinLobbyFailedEventHandler(string cause);

	[Signal] public delegate void PlayerJoinedEventHandler(Dictionary<string, Variant> playerData);
	[Signal] public delegate void PlayerLeftEventHandler(Dictionary<string, Variant> playerData);

	[Signal] public delegate void SpawnNodeSucceedEventHandler(Dictionary<string, Variant> spawnPayload);
	[Signal] public delegate void SpawnNodeFailedEventHandler(string cause);

	[Signal] public delegate void DespawnNodeSucceedEventHandler(string nodePath);
	[Signal] public delegate void DespawnNodeFailedEventHandler(string cause);


	public Node GameHolder => GetNode("/root/GameHolder");
	public Node LobbyNode => GetNode($"/root/GameHolder/Lobby{CurrentLobby[MattohaLobbyKeys.Id]}");



	public Dictionary<string, Variant> CurrentPlayer { get; set; } = new();
	public Dictionary<string, Variant> CurrentLobby { get; set; } = new();
	public Array<Dictionary<string, Variant>> CurrentLobbyPlayers { get; set; } = new();

	public bool ShouldReplicate { get; private set; }
	public void EnableReplication() => ShouldReplicate = true;
	public void DisableReplication() => ShouldReplicate = false;

	private MattohaSystem _system;


	public override void _Ready()
	{
		_system = GetParent<MattohaSystem>();
		Multiplayer.ConnectedToServer += () => EmitSignal(SignalName.ConnectedToServer);
		_system.ClientRpcRecieved += OnClientRpcRecieved;
		base._Ready();
	}


	private void RpcRegisterPlayer(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		CurrentPlayer = payload;
		EmitSignal(SignalName.PlayerRegistered, CurrentPlayer);
#endif
	}


	public void SetPlayerData(Dictionary<string, Variant> playerData)
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.SetPlayerData), playerData);
#endif
	}


	private void RpcSetPlayerData(Dictionary<string, Variant> playerData)
	{
#if MATTOHA_CLIENT
		CurrentPlayer = playerData;
		EmitSignal(SignalName.SetPlayerDataSucceed, CurrentPlayer);
#endif
	}


	public void CreateLobby(Dictionary<string, Variant> lobbyData, string lobbySceneFile)
	{
#if MATTOHA_CLIENT
		lobbyData[MattohaLobbyKeys.LobbySceneFile] = lobbySceneFile;
		_system.SendReliableServerRpc(nameof(ServerRpc.CreateLobby), lobbyData);
#endif
	}


	private void RpcCreateLobby(Dictionary<string, Variant> lobbyData)
	{
#if MATTOHA_CLIENT
		CurrentLobby = lobbyData;
		EmitSignal(SignalName.CreateLobbySucceed, CurrentLobby);
#endif
	}


	public void LoadAvailableLobbies()
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.LoadAvailableLobbies), null);
#endif
	}


	private void RpcLoadAvailableLobbies(Dictionary<string, Variant> lobbiesPayload)
	{
#if MATTOHA_CLIENT
		Array<Dictionary<string, Variant>> lobbies = lobbiesPayload["Lobbies"].AsGodotArray<Dictionary<string, Variant>>();
		EmitSignal(SignalName.LoadAvailableLobbiesSucceed, lobbies);
#endif
	}


	public void JoinLobby(int lobbyId)
	{
#if MATTOHA_CLIENT
		var payload = new Dictionary<string, Variant>()
		{
			{ MattohaLobbyKeys.Id, lobbyId },
		};
		_system.SendReliableServerRpc(nameof(ServerRpc.JoinLobby), payload);
#endif
	}


	private void RpcJoinLobby(Dictionary<string, Variant> joinedLobbyData)
	{
#if MATTOHA_CLIENT
		CurrentLobby = joinedLobbyData;
		EmitSignal(SignalName.JoinLobbySucceed, CurrentLobby);
#endif
	}


	public void StartGame()
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.StartGame), null);
#endif
	}


	private void RpcStartGame()
	{
#if MATTOHA_CLIENT
		CurrentLobby[MattohaLobbyKeys.IsGameStarted] = true;
		EmitSignal(SignalName.StartGameSucceed, CurrentLobby);
#endif
	}

	public void LoadLobbyPlayers()
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.LoadLobbyPlayers), null);
#endif
	}


	private void RpcLoadLobbyPlayers(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		CurrentLobbyPlayers = payload["Players"].AsGodotArray<Dictionary<string, Variant>>();
		GD.Print("Lobby Players: ", CurrentLobbyPlayers);
		EmitSignal(SignalName.LoadLobbyPlayersSucceed, CurrentLobbyPlayers);
#endif
	}


	private void RpcSpawnNode(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.SpawnNodeSucceed, payload);
#endif
	}


	private void OnClientRpcRecieved(string methodName, Dictionary<string, Variant> payload, long sender)
	{
#if MATTOHA_SERVER
		switch (methodName)
		{
			case nameof(ClientRpc.RegisterPlayer):
				RpcRegisterPlayer(payload);
				break;
			case nameof(ClientRpc.SetPlayerData):
				RpcSetPlayerData(payload);
				break;
			case nameof(ClientRpc.CreateLobby):
				RpcCreateLobby(payload);
				break;
			case nameof(ClientRpc.LoadAvailableLobbies):
				RpcLoadAvailableLobbies(payload);
				break;
			case nameof(ClientRpc.JoinLobby):
				RpcJoinLobby(payload);
				break;
			case nameof(ClientRpc.StartGame):
				RpcStartGame();
				break;
			case nameof(ClientRpc.LoadLobbyPlayers):
				RpcLoadLobbyPlayers(payload);
				break;
			case nameof(ClientRpc.SpawnNode):
				RpcSpawnNode(payload);
				break;
		}
#endif
	}

}
