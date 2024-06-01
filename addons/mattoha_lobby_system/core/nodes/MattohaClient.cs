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




	public Node GameHolder => GetNode("/root/GameHolder");
	public Node LobbyNode => GetNode($"/root/GameHolder/Lobby{CurrentLobby[MattohaLobbyKeys.Id]}");



	public Dictionary<string, Variant> CurrentPlayer { get; private set; } = new();
	public Dictionary<string, Variant> CurrentLobby { get; private set; } = new();
	public Array<Dictionary<string, Variant>> CurrentLobbyPlayers { get; private set; } = new();

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

	public Array<long> GetLobbyPlayerIds()
	{
		var ids = new Array<long>() { 1 };
		foreach (var pl in CurrentLobbyPlayers)
		{
			ids.Add(pl[MattohaPlayerKeys.Id].AsInt64());
		}
		return ids;
	}

	public bool IsPlayerInLobby(long playerId)
	{
		if (playerId == 1)
			return true;
		foreach (var pl in CurrentLobbyPlayers)
		{
			if (pl[MattohaPlayerKeys.Id].AsInt64() == playerId)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsPlayerInMyTeam(long playerId)
	{
		if (playerId == 1)
			return true;
		foreach (var pl in CurrentLobbyPlayers)
		{
			if (pl[MattohaPlayerKeys.Id].AsInt32() == playerId)
			{
				return pl[MattohaPlayerKeys.TeamId].AsInt32() == CurrentPlayer[MattohaPlayerKeys.TeamId].AsInt32();
			}
		}
		return false;
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
		EmitSignal(SignalName.LoadLobbyPlayersSucceed, CurrentLobbyPlayers);
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
		}
#endif
	}

}
