using Godot;
using Godot.Collections;
using Mattoha.Core.Demo;
using Mattoha.Core.Utils;
using System;

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


	[Signal] public delegate void SetLobbyDataSucceedEventHandler(Dictionary<string, Variant> lobbyData);
	[Signal] public delegate void SetLobbyDataFailedEventHandler(string cause);

	[Signal] public delegate void SetLobbyOwnerSucceedEventHandler(Dictionary<string, Variant> lobbyData);
	[Signal] public delegate void SetLobbyOwnerFailedEventHandler(string cause);

	[Signal] public delegate void StartGameSucceedEventHandler(Dictionary<string, Variant> lobbyData);
	[Signal] public delegate void StartGameFailedEventHandler(string cause);

	[Signal] public delegate void JoinLobbySucceedEventHandler(Dictionary<string, Variant> lobbyData);
	[Signal] public delegate void JoinLobbyFailedEventHandler(string cause);
	[Signal] public delegate void LeaveLobbySucceedEventHandler();

	[Signal] public delegate void JoinTeamSucceedEventHandler(int newTeam);
	[Signal] public delegate void JoinTeamFailedEventHandler(string cause);


	[Signal] public delegate void NewPlayerJoinedEventHandler(Dictionary<string, Variant> playerData);
	[Signal] public delegate void JoinedPlayerUpdatedEventHandler(Dictionary<string, Variant> playerData);
	[Signal] public delegate void PlayerChangedHisTeamEventHandler(Dictionary<string, Variant> playerData);


	[Signal] public delegate void PlayerJoinedEventHandler(Dictionary<string, Variant> playerData);
	[Signal] public delegate void PlayerLeftEventHandler(Dictionary<string, Variant> playerData);

	[Signal] public delegate void SpawnNodeRequestedEventHandler(Dictionary<string, Variant> nodeData);
	[Signal] public delegate void DespawnNodeRequestedEventHandler(Dictionary<string, Variant> nodeData);


	[Signal] public delegate void GlobalMessageRecievedEventHandler(Dictionary<string, Variant> senderData, string message);
	[Signal] public delegate void LobbyMessageRecievedEventHandler(Dictionary<string, Variant> senderData, string message);
	[Signal] public delegate void TeamMessageRecievedEventHandler(Dictionary<string, Variant> senderData, string message);



	public Node GameHolder => GetNode("/root/GameHolder");
	public Node LobbyNode => GetNode($"/root/GameHolder/Lobby{CurrentLobby[MattohaLobbyKeys.Id]}");



	public Dictionary<string, Variant> CurrentPlayer { get; private set; } = new();
	public Dictionary<string, Variant> CurrentLobby { get; private set; } = new();
	public Dictionary<long, Dictionary<string, Variant>> CurrentLobbyPlayers { get; private set; } = new();

	public bool CanReplicate => CurrentPlayer[MattohaPlayerKeys.IsInGamae].AsBool();

	private MattohaSystem _system;


	public override void _Ready()
	{
		_system = GetParent<MattohaSystem>();
		Multiplayer.ConnectedToServer += () => EmitSignal(SignalName.ConnectedToServer);
		_system.ClientRpcRecieved += OnClientRpcRecieved;
		base._Ready();
	}

	public Array<long> GetLobbyPlayersIds()
	{
		var ids = new Array<long>() { 1, Multiplayer.GetUniqueId() };
		foreach (var id in CurrentLobbyPlayers.Keys)
		{
			ids.Add(id);
		}
		return ids;
	}

	public bool IsPlayerInLobby(long playerId)
	{
		if (playerId == 1)
			return true;
		return CurrentLobbyPlayers.ContainsKey(playerId);
	}

	public bool IsPlayerInMyTeam(long playerId)
	{
		if (playerId == 1)
			return true;
		return CurrentLobbyPlayers[playerId][MattohaPlayerKeys.TeamId].AsInt32() == CurrentPlayer[MattohaPlayerKeys.TeamId].AsInt32();
	}

	public bool IsPlayerInGame(long playerId)
	{
		if (playerId == 1)
			return true;
		return CurrentPlayer[MattohaPlayerKeys.IsInGamae].AsBool();
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


	public void SetLobbyData(Dictionary<string, Variant> lobbyData)
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.SetLobbyData), lobbyData);
#endif
	}

	public void SetLobbyOwner(int newOwnerId)
	{
#if MATTOHA_CLIENT
		var payload = new Dictionary<string, Variant> { { MattohaLobbyKeys.OwnerId, newOwnerId } };
		_system.SendReliableServerRpc(nameof(ServerRpc.SetLobbyOwner), payload);
#endif
	}

	private void RpcSetLobbyOwner(Dictionary<string, Variant> lobbyData)
	{
#if MATTOHA_SERVER
		CurrentLobby = lobbyData;
		EmitSignal(SignalName.SetLobbyOwnerSucceed, CurrentLobby);
#endif
	}


	public void RpcSetLobbyData(Dictionary<string, Variant> lobbyData)
	{
#if MATTOHA_CLIENT
		CurrentLobby = lobbyData;
		EmitSignal(SignalName.SetLobbyDataSucceed, CurrentLobby);
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
		var players = new Array<Dictionary<string, Variant>>();
		foreach (var player in payload["Players"].AsGodotArray<Dictionary<string, Variant>>())
		{
			CurrentLobbyPlayers[player[MattohaPlayerKeys.Id].AsInt64()] = player;
			players.Add(player);
		}

		EmitSignal(SignalName.LoadLobbyPlayersSucceed, players);
#endif
	}


	public void SpawnNode(Node node)
	{
#if MATTOHA_CLIENT
		var payload = _system.GenerateNodePayloadData(node);
		_system.SendReliableServerRpc(nameof(ServerRpc.SpawnNode), payload);
#endif
	}


	private void RpcSpawnNode(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.SpawnNodeRequested, payload);
#endif
	}


	public void SpawnLobbyNodes()
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.SpawnLobbyNodes), null);
#endif
	}


	private void RpcSpawnLobbyNodes(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		var nodes = payload["Nodes"].AsGodotArray<Dictionary<string, Variant>>();
		foreach (var node in nodes)
		{
			EmitSignal(SignalName.SpawnNodeRequested, node);
		}
		// why?: because to notify other players to replicate their data, and we are sure that all nodes have been spawned
		SetPlayerData(new Dictionary<string, Variant> { { MattohaPlayerKeys.IsInGamae, true } });
#endif
	}


	public void DespawnNode(Node node)
	{
#if MATTOHA_CLIENT
		var payload = _system.GenerateNodePayloadData(node, true);
		_system.SendReliableServerRpc(nameof(ServerRpc.DespawnNode), payload);
#endif
	}


	private void RpcDespawnNode(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.DespawnNodeRequested, payload);
#endif
	}

	public void DespawnRemovedSceneNodes()
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.DespawnRemovedSceneNodes), null);
#endif
	}


	private void RpcDespawnRemovedSceneNodes(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		var nodes = payload["Nodes"].AsGodotArray<Dictionary<string, Variant>>();
		foreach (var node in nodes)
		{
			EmitSignal(SignalName.DespawnNodeRequested, node);
		}
#endif
	}


	private void RpcNewPlayerJoined(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		var playerId = payload[MattohaPlayerKeys.Id].AsInt64();
		var isPlayerExists = CurrentLobbyPlayers.ContainsKey(playerId);
		if (!isPlayerExists)
		{
			CurrentLobbyPlayers.Add(payload[MattohaPlayerKeys.Id].AsInt64(), payload);
			EmitSignal(SignalName.NewPlayerJoined, CurrentLobbyPlayers[playerId]);
		}
#endif
	}

	private void RpcJoinedPlayerUpdated(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		var playerId = payload[MattohaPlayerKeys.Id].AsInt64();
		CurrentLobbyPlayers[playerId] = payload;
		EmitSignal(SignalName.JoinedPlayerUpdated, payload);
#endif
	}


	public void JoinTeam(int teamId)
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.JoinTeam), new Dictionary<string, Variant> { { "TeamId", teamId } });
#endif
	}


	private void RpcJoinTeam(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		var teamId = payload[MattohaPlayerKeys.TeamId].AsInt32();
		CurrentPlayer[MattohaPlayerKeys.TeamId] = teamId;
		EmitSignal(SignalName.JoinTeamSucceed, teamId);
#endif
	}


	private void RpcPlayerChangedHisTeam(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		var playerId = payload[MattohaPlayerKeys.Id].AsInt64();
		CurrentLobbyPlayers[playerId] = payload;
		EmitSignal(SignalName.PlayerChangedHisTeam, payload);
#endif
	}


	public void SendTeamMessage(string message)
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.SendTeamMessage), new Dictionary<string, Variant> { { "Message", message } });
#endif
	}

	private void RpcSendTeamMessage(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.TeamMessageRecieved, payload["Player"], payload["Message"]);
#endif
	}


	public void SendLobbyMessage(string message)
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.SendLobbyMessage), new Dictionary<string, Variant> { { "Message", message } });
#endif
	}


	private void RpcSendGlobalMessage(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.GlobalMessageRecieved, payload["Player"], payload["Message"]);
#endif
	}


	public void SendGlobalMessage(string message)
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.SendGlobalMessage), new Dictionary<string, Variant> { { "Message", message } });
#endif
	}

	private void RpcSendLobbyMessage(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.LobbyMessageRecieved, payload["Player"], payload["Message"]);
#endif
	}

	public void LeaveLobby()
	{
#if MATTOHA_CLIENT
		CurrentPlayer[MattohaPlayerKeys.JoinedLobbyId] = 0;
		_system.SendReliableServerRpc(nameof(ServerRpc.LeaveLobby), null);
#endif
	}

	private void RpcLeaveLobby()
	{
#if MATTOHA_CLIENT
		CurrentPlayer[MattohaPlayerKeys.JoinedLobbyId] = 0;
		EmitSignal(SignalName.LeaveLobbySucceed);
#endif
	}

	private void RpcPlayerLeft(Dictionary<string, Variant> payload)
	{
#if MATTOHA_SERVER
		var playerId = payload[MattohaPlayerKeys.Id].AsInt64();
		CurrentLobbyPlayers.Remove(playerId);
		EmitSignal(SignalName.PlayerLeft, payload);
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
			case nameof(ClientRpc.SetLobbyData):
				RpcSetLobbyData(payload);
				break;
			case nameof(ClientRpc.SetLobbyOwner):
				RpcSetLobbyOwner(payload);
				break;
			case nameof(ClientRpc.LoadAvailableLobbies):
				RpcLoadAvailableLobbies(payload);
				break;
			case nameof(ClientRpc.JoinLobby):
				RpcJoinLobby(payload);
				break;
			case nameof(ClientRpc.JoinTeam):
				RpcJoinTeam(payload);
				break;
			case nameof(ClientRpc.PlayerChangedHisTeam):
				RpcPlayerChangedHisTeam(payload);
				break;
			case nameof(ClientRpc.NewPlayerJoined):
				RpcNewPlayerJoined(payload);
				break;
			case nameof(ClientRpc.JoinedPlayerUpdated):
				RpcJoinedPlayerUpdated(payload);
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
			case nameof(ClientRpc.DespawnNode):
				RpcDespawnNode(payload);
				break;
			case nameof(ClientRpc.SpawnLobbyNodes):
				RpcSpawnLobbyNodes(payload);
				break;
			case nameof(ClientRpc.DespawnRemovedSceneNodes):
				RpcDespawnRemovedSceneNodes(payload);
				break;
			case nameof(ClientRpc.SendGlobalMessage):
				RpcSendGlobalMessage(payload);
				break;
			case nameof(ClientRpc.SendLobbyMessage):
				RpcSendLobbyMessage(payload);
				break;
			case nameof(ClientRpc.SendTeamMessage):
				RpcSendTeamMessage(payload);
				break;
			case nameof(ClientRpc.LeaveLobby):
				RpcLeaveLobby();
				break;
			case nameof(ClientRpc.PlayerLeft):
				RpcPlayerLeft(payload);
				break;
		}
#endif
	}
}
