using Godot;
using Godot.Collections;
using Mattoha.Core.Demo;
using Mattoha.Core.Utils;
using System;

namespace Mattoha.Nodes;
public partial class MattohaClient : Node
{
	/// <summary>
	/// Emmited when connection to server is succeed.
	/// </summary>
	[Signal] public delegate void ConnectedToServerEventHandler();

	/// <summary>
	/// Emmited when player is connected and registered in server memory.
	/// </summary>
	/// <param name="playerData">Initial player data.</param>
	[Signal] public delegate void PlayerRegisteredEventHandler(Dictionary<string, Variant> playerData);

	/// <summary>
	/// Emmited when loading lobby players loaded successfully.
	/// </summary>
	/// <param name="players">Joined players in lobby.</param>
	[Signal] public delegate void LoadLobbyPlayersSucceedEventHandler(Array<Dictionary<string, Variant>> players);

	/// <summary>
	/// Emmited when loading lobby player failed.
	/// </summary>
	/// <param name="cause">Fail cause.</param>
	[Signal] public delegate void LoadLobbyPlayersFailedEventHandler(string cause);

	/// <summary>
	/// Emmited when available lobbies loaded successfully.
	/// </summary>
	/// <param name="lobbies">Available lobbies</param>
	[Signal] public delegate void LoadAvailableLobbiesSucceedEventHandler(Array<Dictionary<string, Variant>> lobbies);

	/// <summary>
	/// Emmited when available lobbies failed to load.
	/// </summary>
	/// <param name="cause">fail cause</param>
	[Signal] public delegate void LoadAvailableLobbiesFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when setting player data succeeds.
	/// </summary>
	/// <param name="playerData">The updated player data.</param>
	[Signal] public delegate void SetPlayerDataSucceedEventHandler(Dictionary<string, Variant> playerData);

	/// <summary>
	/// Emitted when setting player data fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void SetPlayerDataFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when creating a lobby succeeds.
	/// </summary>
	/// <param name="lobbyData">The data of the created lobby.</param>
	[Signal] public delegate void CreateLobbySucceedEventHandler(Dictionary<string, Variant> lobbyData);

	/// <summary>
	/// Emitted when creating a lobby fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void CreateLobbyFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when setting lobby data succeeds.
	/// </summary>
	/// <param name="lobbyData">The updated lobby data.</param>
	[Signal] public delegate void SetLobbyDataSucceedEventHandler(Dictionary<string, Variant> lobbyData);

	/// <summary>
	/// Emitted when setting lobby data fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void SetLobbyDataFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when setting the lobby owner succeeds.
	/// </summary>
	/// <param name="lobbyData">The updated lobby data with the new owner.</param>
	[Signal] public delegate void SetLobbyOwnerSucceedEventHandler(Dictionary<string, Variant> lobbyData);

	/// <summary>
	/// Emitted when setting the lobby owner fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void SetLobbyOwnerFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when starting the game succeeds.
	/// </summary>
	/// <param name="lobbyData">The updated lobby data after starting the game.</param>
	[Signal] public delegate void StartGameSucceedEventHandler(Dictionary<string, Variant> lobbyData);

	/// <summary>
	/// Emitted when starting the game fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void StartGameFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when joining a lobby succeeds.
	/// </summary>
	/// <param name="lobbyData">The data of the joined lobby.</param>
	[Signal] public delegate void JoinLobbySucceedEventHandler(Dictionary<string, Variant> lobbyData);

	/// <summary>
	/// Emitted when joining a lobby fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void JoinLobbyFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when leaving a lobby succeeds.
	/// </summary>
	[Signal] public delegate void LeaveLobbySucceedEventHandler();

	/// <summary>
	/// Emitted when joining a team succeeds.
	/// </summary>
	/// <param name="newTeam">The team the player joined.</param>
	[Signal] public delegate void JoinTeamSucceedEventHandler(int newTeam);

	/// <summary>
	/// Emitted when joining a team fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void JoinTeamFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when a new player joins.
	/// </summary>
	/// <param name="playerData">The data of the new player.</param>
	[Signal] public delegate void NewPlayerJoinedEventHandler(Dictionary<string, Variant> playerData);

	/// <summary>
	/// Emitted when a joined player updates their information.
	/// </summary>
	/// <param name="playerData">The updated player data.</param>
	[Signal] public delegate void JoinedPlayerUpdatedEventHandler(Dictionary<string, Variant> playerData);

	/// <summary>
	/// Emitted when a player changes their team.
	/// </summary>
	/// <param name="playerData">The data of the player who changed teams.</param>
	[Signal] public delegate void PlayerChangedHisTeamEventHandler(Dictionary<string, Variant> playerData);

	/// <summary>
	/// Emitted when a player joins current lobby.
	/// </summary>
	/// <param name="playerData">The data of the player who joined.</param>
	[Signal] public delegate void PlayerJoinedEventHandler(Dictionary<string, Variant> playerData);

	/// <summary>
	/// Emitted when a player leaves.
	/// </summary>
	/// <param name="playerData">The data of the player who left.</param>
	[Signal] public delegate void PlayerLeftEventHandler(Dictionary<string, Variant> playerData);

	/// <summary>
	/// Emitted when a node spawn is requested.
	/// </summary>
	/// <param name="nodeData">The data of the node to be spawned.</param>
	[Signal] public delegate void SpawnNodeRequestedEventHandler(Dictionary<string, Variant> nodeData);

	/// <summary>
	/// Emitted when node spawning fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void SpawnNodeFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when spawning lobby nodes fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void SpawnLobbyNodesFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when a node despawn is requested.
	/// </summary>
	/// <param name="nodeData">The data of the node to be despawned.</param>
	[Signal] public delegate void DespawnNodeRequestedEventHandler(Dictionary<string, Variant> nodeData);

	/// <summary>
	/// Emitted when node despawning fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void DespawnNodeFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when despawning removed scene nodes fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void DespawnRemovedSceneNodesFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when a global message is received.
	/// </summary>
	/// <param name="senderData">The data of the message sender.</param>
	/// <param name="message">The received message.</param>
	[Signal] public delegate void GlobalMessageRecievedEventHandler(Dictionary<string, Variant> senderData, string message);

	/// <summary>
	/// Emitted when sending a global message fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void GlobalMessageFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when a lobby message is received.
	/// </summary>
	/// <param name="senderData">The data of the message sender.</param>
	/// <param name="message">The received message.</param>
	[Signal] public delegate void LobbyMessageRecievedEventHandler(Dictionary<string, Variant> senderData, string message);

	/// <summary>
	/// Emitted when sending a lobby message fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void LobbyMessageFailedEventHandler(string cause);

	/// <summary>
	/// Emitted when a team message is received.
	/// </summary>
	/// <param name="senderData">The data of the message sender.</param>
	/// <param name="message">The received message.</param>
	[Signal] public delegate void TeamMessageRecievedEventHandler(Dictionary<string, Variant> senderData, string message);

	/// <summary>
	/// Emitted when sending a team message fails.
	/// </summary>
	/// <param name="cause">The cause of the failure.</param>
	[Signal] public delegate void TeamMessageFailedEventHandler(string cause);


	/// <summary>
	/// Returns The GameHolder node in game tree.
	/// </summary>
	public Node GameHolder => GetNode("/root/GameHolder");

	/// <summary>
	/// Returns The LobbyNode (Current arena game scene of lobby) in game tree.
	/// </summary>
	public Node LobbyNode => GetTree().Root.HasNode($"/root/GameHolder/Lobby{CurrentLobby[MattohaLobbyKeys.Id]}") ? GetNode($"/root/GameHolder/Lobby{CurrentLobby[MattohaLobbyKeys.Id]}") : null;

	/// <summary>
	/// Returns the current player data synced with all players, "PrivateProps" list is not synced with others.
	/// </summary>
	public Dictionary<string, Variant> CurrentPlayer { get; private set; } = new();

	/// <summary>
	/// Returns the current player data synced with all players in lobby, "PrivateProps" list is not synced with others.
	/// </summary>
	public Dictionary<string, Variant> CurrentLobby { get; private set; } = new();

	/// <summary>
	/// Returns the dictionary containing joined players in current lobby, without PrivateProps items.
	/// </summary>
	public Dictionary<long, Dictionary<string, Variant>> CurrentLobbyPlayers { get; private set; } = new();

	/// <summary>
	/// Returns true if player "IsInGame" proeprty is true, IsInGame will be true when player sync spawned lobby nodes by others.
	/// </summary>
	public bool CanReplicate => CurrentPlayer[MattohaPlayerKeys.IsInGame].AsBool();

	private MattohaSystem _system;

	public override void _Ready()
	{
		_system = GetParent<MattohaSystem>();
		Multiplayer.ConnectedToServer += () => EmitSignal(SignalName.ConnectedToServer);
		_system.ClientRpcRecieved += OnClientRpcRecieved;
		base._Ready();
	}

	/// <summary>
	/// Returns joined lobby players Ids.
	/// </summary>
	/// <returns>IDs list.</returns>
	public Array<long> GetLobbyPlayersIds()
	{
		var ids = new Array<long>() { 1 };
		foreach (var id in CurrentLobbyPlayers.Keys)
		{
			ids.Add(id);
		}
		return ids;
	}

	/// <summary>
	/// Check if peer id is joined in lobby or not.
	/// </summary>
	/// <param name="playerId">peer id</param>
	/// <returns>true if peerId is joined.</returns>
	public bool IsPlayerInLobby(long playerId)
	{
		if (playerId == 1 || playerId == 0)
			return true;
		if (!CurrentLobbyPlayers.ContainsKey(playerId))
			return false;
		return CurrentLobbyPlayers.ContainsKey(playerId);
	}

	/// <summary>
	/// Check if the player id is in same team.
	/// </summary>
	/// <param name="playerId">player id to check.</param>
	/// <returns>true if in same team.</returns>
	public bool IsPlayerInMyTeam(long playerId)
	{
		if (playerId == 1 || playerId == 0)
			return true;
		if (!CurrentLobbyPlayers.ContainsKey(playerId))
			return false;
		return CurrentLobbyPlayers[playerId][MattohaPlayerKeys.TeamId].AsInt32() == CurrentPlayer[MattohaPlayerKeys.TeamId].AsInt32();
	}

	/// <summary>
	/// Returns true if player entered the game scene and spawned lobby nodes, this will be used under the hood for replication purposes.
	/// </summary>
	/// <param name="playerId">player id to check.</param>
	/// <returns>true if entered the game.</returns>
	public bool IsPlayerInGame(long playerId)
	{
		if (playerId == 1)
			return true;
		return CurrentPlayer[MattohaPlayerKeys.IsInGame].AsBool();
	}

	private void RpcRegisterPlayer(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		CurrentPlayer = payload;
		EmitSignal(SignalName.PlayerRegistered, CurrentPlayer);
#endif
	}

	/// <summary>
	/// Set Player data in server, this will emit SetPlayerDataSucceed, for current client 
	/// and emmit JoinedPlayerUpdated for other lobby players.
	/// <br/><br/>
	/// Base player keys you can edit in addition to your custom keys:
	/// - Username {string}
	/// - TeamId {int}
	/// - PrivateProps {Godot Array of string}
	/// - ChatProps {Godot Array of string}
	/// </summary>
	/// <param name="playerData">A dictionary containing the keys to update, its ok to put only one key, no need for full player data.</param>
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

	private void RpcSetPlayerDataFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.SetPlayerDataFailed, payload["Message"].AsString());
#endif
	}


	/// <summary>
	/// Create new lobby on server, this will emmit "CreateLobbySucceed and SetPlayerDataSucceed" or "CreateLobbyFailed" for creator,
	/// and "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled.
	/// <br/><br/>
	/// Base lobby keys you can edit in addition to your custom keys:
	/// - Name {string}
	/// - MaxPlayers {int}
	/// - PrivateProps {Godot Array of string}
	/// - LobbySceneFile {string}
	/// </summary>
	/// <param name="lobbyData">Lobby Data to create.</param>
	/// <param name="lobbySceneFile">The scene file for lobby game, for example: "res://maps/arean.tscn".</param>
	public void CreateLobby(Dictionary<string, Variant> lobbyData, string lobbySceneFile)
	{
#if MATTOHA_CLIENT
		lobbyData[MattohaLobbyKeys.LobbySceneFile] = lobbySceneFile;
		_system.SendReliableServerRpc(nameof(ServerRpc.CreateLobby), lobbyData);
#endif
	}


	/// <summary>
	/// Set Lobby data and sync it, this will emmit "SetLobbyDataFailed" for caller user if Setting Data failed, 
	/// and sync the data for all joined players - including caller user - then emmit "SetLobbyDataSucceed" for them,
	/// and "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled.
	/// <br/><br/>
	/// Base lobby keys you can edit in addition to your custom keys:
	/// - Name {string}
	/// - MaxPlayers {int}
	/// - PrivateProps {Godot Array of string}
	/// - LobbySceneFile {string}
	/// </summary>
	/// <param name="lobbyData">A dictionary containing the keys you want to update, its ok to use one key, no need for full lobby data.</param>
	public void SetLobbyData(Dictionary<string, Variant> lobbyData)
	{
#if MATTOHA_CLIENT
		_system.SendReliableServerRpc(nameof(ServerRpc.SetLobbyData), lobbyData);
#endif
	}


	private void RpcCreateLobby(Dictionary<string, Variant> lobbyData)
	{
#if MATTOHA_CLIENT
		CurrentLobby = lobbyData;
		EmitSignal(SignalName.CreateLobbySucceed, CurrentLobby);
#endif
	}

	private void RpcCreateLobbyFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.CreateLobbyFailed, payload["Message"].AsString());
#endif
	}


	/// <summary>
	/// Change lobby owner, this will emmit "SetLobbyOwnerSucceed" for all joined players including caller user if changing owner is succeed, 
	/// and emmit "SetLobbyOwnerFailed" for caller user if setting owner fails, ONLY owner of the lobby can set new lobby owner.
	/// </summary>
	/// <param name="newOwnerId">New owner id</param>
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


	private void RpcSetLobbyOwnerFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_SERVER
		EmitSignal(SignalName.SetLobbyOwnerFailed, payload["Message"].AsString());
#endif
	}


	private void RpcSetLobbyData(Dictionary<string, Variant> lobbyData)
	{
#if MATTOHA_CLIENT
		CurrentLobby = lobbyData;
		EmitSignal(SignalName.SetLobbyDataSucceed, CurrentLobby);
#endif
	}


	private void RpcSetLobbyDataFailed(Dictionary<string, Variant> lobbyData)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.SetLobbyDataFailed, lobbyData["Message"].AsString());
#endif
	}


	/// <summary>
	/// Load Available lobbies, this will emmit "LoadAvailableLobbiesSucceed" or "LoadAvailableLobbiesFailed" for caller user.
	/// </summary>
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

	private void RpcLoadAvailableLobbiesFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.LoadAvailableLobbiesFailed, payload["Message"].AsString());
#endif
	}


	/// <summary>
	/// Join lobby by it's ID, this will emmit "JoinLobbySucceed" or "JoinLobbyFailed" for caller user,
	/// and "NewPlayerJoined" for other joined players,
	/// and "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled.
	/// </summary>
	/// <param name="lobbyId">Lobby id to join to.</param>
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


	private void RpcJoinLobbyFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.JoinLobbyFailed, payload["Message"].AsString());
#endif
	}


	/// <summary>
	/// Start Lobby Game, this eill emmit "StartGameSucceed" for all joined players or "StartGameFailed" for caller user,
	/// and "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled.
	/// </summary>
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


	private void RpcStartGameFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.StartGameFailed, payload["Message"].AsString());
#endif
	}


	/// <summary>
	/// Load players data that joined in same lobby, this will emmit "LoadLobbyPlayersSucceed" or "LoadLobbyPlayersFailed"
	/// for the caller user.
	/// </summary>
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


	private void RpcLoadLobbyPlayersFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.LoadLobbyPlayersFailed, payload["Message"].AsString());
#endif
	}


	/// <summary>
	/// Spawn node for all joined players in same lobby or same team, this will emmit "SpawnNodeRequested" for joined players or players in teams,
	/// "SpawnNodeFailed" will be emmited and node will be locally despawned if spawning node failed.
	/// </summary>
	/// <param name="node">Node object (should be existing in tree)</param>
	/// <param name="teamOnly">true if spawning should be for team only.</param>
	public void SpawnNode(Node node, bool teamOnly = false)
	{
#if MATTOHA_CLIENT
		var payload = _system.GenerateNodePayloadData(node);
		payload[MattohaSpawnKeys.TeamOnly] = teamOnly;
		_system.SendReliableServerRpc(nameof(ServerRpc.SpawnNode), payload);
#endif
	}


	private void RpcSpawnNode(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.SpawnNodeRequested, payload);
#endif
	}


	private void RpcSpawnNodeFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		var nodePath = payload["NodePath"].AsString();
		CallDeferred(nameof(DespawnNodeDeferred), nodePath);
		EmitSignal(SignalName.SpawnNodeFailed, payload["Message"].AsString());
#endif
	}

	private void DespawnNodeDeferred(string nodePath)
	{
#if MATTOHA_CLIENT
		if (GetTree().Root.HasNode(nodePath))
		{
			GetNode(nodePath).QueueFree();
		}
#endif
	}


	/// <summary>
	/// Spawn all lobby nodes tha has been spawned by other players, team only nodes will not be spawned until player in same team, 
	/// this method will emmit "SpawnLobbyNodesSucceed" or  "SpawnLobbyNodesFailed".
	/// </summary>
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
			_system.SpawnNodeFromPayload(node);
		}
		// why?: because to notify other players to replicate their data, and we are sure that all nodes have been spawned
		SetPlayerData(new Dictionary<string, Variant> { { MattohaPlayerKeys.IsInGame, true } });
#endif
	}


	private void RpcSpawnLobbyNodesFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.SpawnLobbyNodesFailed, payload["Message"].AsString());
#endif
	}


	/// <summary>
	/// Despawn node and sync that with all players, this will emmit "DespawnNodeRequested" for all players and "DespawnNodeFailed" for caller user
	/// if despawning fails and then respawning the node in case if player destroyed it.
	/// </summary>
	/// <param name="node"></param>
	public void DespawnNode(Node node)
	{
#if MATTOHA_CLIENT
		var payload = _system.GenerateNodePayloadData(node);
		_system.SendReliableServerRpc(nameof(ServerRpc.DespawnNode), payload);
#endif
	}

	private void RpcDespawnNode(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.DespawnNodeRequested, payload);
#endif
	}


	private void RpcDespawnNodeFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		var nodePayload = payload["NodePayload"].AsGodotDictionary<string, Variant>();
		CallDeferred(nameof(RespawnNodeDeferred), nodePayload);
		EmitSignal(SignalName.DespawnNodeFailed, payload["Message"].AsString());
#endif
	}

	private void RespawnNodeDeferred(Dictionary<string, Variant> nodePayload)
	{
#if MATTOHA_CLIENT
		_system.SpawnNodeFromPayload(nodePayload);
#endif
	}


	/// <summary>
	/// Despawn All scene nodes that despawned during game player, used by gameholder when new player enter the game scene,
	/// this will emmit "DespawnRemovedSceneNodesSucceed" or "DespawnRemovedSceneNodesFailed".
	/// </summary>
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


	private void RpcDespawnRemovedSceneNodesFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.DespawnRemovedSceneNodesFailed, payload["Message"].AsString());
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


	/// <summary>
	/// Join or change a team, this will emmit "JoinTeamSucceed" or "JoinTeamFailed" if joingin failed,
	/// in addition, "PlayerChangedHisTeam" will be emmited on all lobby players including the player itself.
	/// </summary>
	/// <param name="teamId">Team ID to join to.</param>
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


	private void RpcJoinTeamFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.JoinTeamFailed, payload["Message"].AsString());
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


	/// <summary>
	/// Send a message for team members, this will emmit "TeamMessageSucceed" for all players or "TeamMessageFailed" for caller user.
	/// </summary>
	/// <param name="message">message to send.</param>
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


	private void RpcSendTeamMessageFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.TeamMessageFailed, payload["Message"].AsString());
#endif
	}


	/// <summary>
	/// Send a message for lobby members, this will emmit "LobbyMessageSucceed" for all players or "LobbyMessageFailed" for caller user.
	/// </summary>
	/// <param name="message">message to send.</param>
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


	private void RpcSendGlobalMessageFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.GlobalMessageFailed, payload["Message"].AsString());
#endif
	}


	/// <summary>
	/// Send a global message for all online users, this will emmit "GlobalMessageSucceed" for all players or "GlobalMessageFailed" for caller user.
	/// </summary>
	/// <param name="message">message to send.</param>
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


	private void RpcSendLobbyMessageFailed(Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.LobbyMessageFailed, payload["Message"].ToString());
#endif
	}

	/// <summary>
	/// Leave joined lobby, this will emmit "LeaveLobbySucceed" for caller user and "PlayerLeft" for all joined players.
	/// </summary>
	public void LeaveLobby()
	{
#if MATTOHA_CLIENT
		CurrentPlayer[MattohaPlayerKeys.JoinedLobbyId] = 0;
		CurrentLobbyPlayers = new();
		CurrentLobby = new();
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
			case nameof(ClientRpc.SetPlayerDataFailed):
				RpcSetPlayerDataFailed(payload);
				break;


			case nameof(ClientRpc.CreateLobby):
				RpcCreateLobby(payload);
				break;
			case nameof(ClientRpc.CreateLobbyFailed):
				RpcCreateLobbyFailed(payload);
				break;


			case nameof(ClientRpc.SetLobbyData):
				RpcSetLobbyData(payload);
				break;
			case nameof(ClientRpc.SetLobbyDataFailed):
				RpcSetLobbyDataFailed(payload);
				break;


			case nameof(ClientRpc.SetLobbyOwner):
				RpcSetLobbyOwner(payload);
				break;
			case nameof(ClientRpc.SetLobbyOwnerFailed):
				RpcSetLobbyOwnerFailed(payload);
				break;


			case nameof(ClientRpc.LoadAvailableLobbies):
				RpcLoadAvailableLobbies(payload);
				break;
			case nameof(ClientRpc.LoadAvailableLobbiesFailed):
				RpcLoadAvailableLobbiesFailed(payload);
				break;


			case nameof(ClientRpc.JoinLobby):
				RpcJoinLobby(payload);
				break;
			case nameof(ClientRpc.JoinLobbyFailed):
				RpcJoinLobbyFailed(payload);
				break;


			case nameof(ClientRpc.JoinTeam):
				RpcJoinTeam(payload);
				break;
			case nameof(ClientRpc.JoinTeamFailed):
				RpcJoinTeamFailed(payload);
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
			case nameof(ClientRpc.StartGameFailed):
				RpcStartGameFailed(payload);
				break;


			case nameof(ClientRpc.LoadLobbyPlayers):
				RpcLoadLobbyPlayers(payload);
				break;
			case nameof(ClientRpc.LoadLobbyPlayersFailed):
				RpcLoadLobbyPlayersFailed(payload);
				break;


			case nameof(ClientRpc.SpawnNode):
				RpcSpawnNode(payload);
				break;
			case nameof(ClientRpc.SpawnNodeFailed):
				RpcSpawnNodeFailed(payload);
				break;


			case nameof(ClientRpc.DespawnNode):
				RpcDespawnNode(payload);
				break;
			case nameof(ClientRpc.DespawnNodeFailed):
				RpcDespawnNodeFailed(payload);
				break;


			case nameof(ClientRpc.SpawnLobbyNodes):
				RpcSpawnLobbyNodes(payload);
				break;
			case nameof(ClientRpc.SpawnLobbyNodesFailed):
				RpcSpawnLobbyNodesFailed(payload);
				break;


			case nameof(ClientRpc.DespawnRemovedSceneNodes):
				RpcDespawnRemovedSceneNodes(payload);
				break;
			case nameof(ClientRpc.DespawnRemovedSceneNodesFailed):
				RpcDespawnRemovedSceneNodesFailed(payload);
				break;


			case nameof(ClientRpc.SendGlobalMessage):
				RpcSendGlobalMessage(payload);
				break;
			case nameof(ClientRpc.SendGlobalMessageFailed):
				RpcSendGlobalMessageFailed(payload);
				break;


			case nameof(ClientRpc.SendLobbyMessage):
				RpcSendLobbyMessage(payload);
				break;
			case nameof(ClientRpc.SendLobbyMessageFailed):
				RpcSendLobbyMessageFailed(payload);
				break;


			case nameof(ClientRpc.SendTeamMessage):
				RpcSendTeamMessage(payload);
				break;
			case nameof(ClientRpc.SendTeamMessageFailed):
				RpcSendTeamMessageFailed(payload);
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
