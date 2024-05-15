using Godot;
using MattohaLobbySystem.Core.Enums;
using MattohaLobbySystem.Core.Interfaces;
using MattohaLobbySystem.Core.Models;
using MattohaLobbySystem.Core.Utils;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace MattohaLobbySystem.Core.Nodes;
public partial class MattohaClientBase : Node, IMattohaClientRpc, IMattohaServerRpc
{
	#region Signals
	/// <summary>
	/// Emmited when a connection to server is succeed.
	/// </summary>
	[Signal] public delegate void ConnectedToServerEventHandler();

	/// <summary>
	/// Emmited when a connection to server failed.
	/// </summary>
	[Signal] public delegate void ConnectionFailedEventHandler();

	/// <summary>
	/// Emmited when server disconnected.
	/// </summary>
	[Signal] public delegate void ServerDisconnectedEventHandler();

	/// <summary>
	/// Emmited when current player data is updated.
	/// </summary>
	/// <param name="player">MattohaSignal with value of player data as JsonObject.</param>
	[Signal] public delegate void CurrentPlayerUpdatedEventHandler(MattohaSignal<JsonObject> player);

	/// <summary>
	/// Emmited when a new lobby is created successfully.
	/// </summary>
	/// <param name="lobby">MattohaSignal with value of lobby data as JsonObject.</param>
	[Signal] public delegate void NewLobbyCreatedEventHandler(MattohaSignal<JsonObject> lobby);

	/// <summary>
	/// Emmited when the list of available lobbies is updated. 
	/// </summary>
	/// <param name="lobbies">MatohaSignal with value of Available lobbies as json objects.</param>
	[Signal] public delegate void AvailableLobbiesRefreshedEventHandler(MattohaSignal<List<JsonObject>> lobbies);

	/// <summary>
	/// Emmited when joining to lobby succeeed.
	/// </summary>
	/// <param name="lobby">MattohaSignal with value of lobby data as JsonObject.</param>
	[Signal] public delegate void JoinLobbySucceedEventHandler(MattohaSignal<JsonObject> lobby);

	/// <summary>
	/// Emmited when new player joiend current lobby.
	/// </summary>
	/// <param name="player">MattohaSignal with value of lobby data as JsonObject.</param>
	[Signal] public delegate void NewPlayerJoinedLobbyEventHandler(MattohaSignal<JsonObject> player);

	/// <summary>
	/// Emmited when joined lobby is updated.
	/// </summary>
	/// <param name="lobby">MattohaSignal with value of lobby data as JsonObject.</param>
	[Signal] public delegate void JoinedLobbyRefreshedEventHandler(MattohaSignal<JsonObject> lobby);

	/// <summary>
	/// Emmited when joined players list in lobby is updated.
	/// </summary>
	/// <param name="players">MattohaSignal with dictionary of joined players.</param>
	[Signal] public delegate void JoinedPlayersRefreshedEventHandler(MattohaSignal<Dictionary<long, JsonObject>> players);

	/// <summary>
	/// Emmited when a new team message is recieved.
	/// </summary>
	/// <param name="message">message content</param>
	/// <param name="player">player data</param>
	[Signal] public delegate void TeamMessageReceivedEventHandler(MattohaSignal<string> message, MattohaSignal<JsonObject> player);

	/// <summary>
	/// Emmited when a new lobby message is recieved.
	/// </summary>
	/// <param name="message">message content</param>
	/// <param name="player">player data</param>
	[Signal] public delegate void LobbyMessageReceivedEventHandler(MattohaSignal<string> message, MattohaSignal<JsonObject> player);

	/// <summary>
	/// Emmited when a new global message is recieved.
	/// </summary>
	/// <param name="message">message content</param>
	/// <param name="player">player data</param>
	[Signal] public delegate void GlobalMessageReceivedEventHandler(MattohaSignal<string> message, MattohaSignal<JsonObject> player);

	/// <summary>
	/// Emmited when a joined lobby game started.
	/// </summary>
	[Signal] public delegate void GameStartedEventHandler();

	/// <summary>
	/// Emmited when a player left a lobby.
	/// </summary>
	/// <param name="player">player data</param>
	[Signal] public delegate void PlayerLeftLobbyEventHandler(MattohaSignal<JsonObject> player);

	/// <summary>
	/// Emmited when a new node should be spawned.
	/// </summary>
	/// <param name="nodeInfo">node info</param>
	[Signal] public delegate void NodeSpawnRequestedEventHandler(MattohaSignal<MattohaSpawnNodeInfo> nodeInfo);

	/// <summary>
	/// Emmited when a node hsould be despawned.
	/// </summary>
	/// <param name="nodePath"></param>
	[Signal] public delegate void NodeDespawnRequestedEventHandler(MattohaSignal<string> nodePath);

	/// <summary>
	/// Emmited when joining to lobby failed.
	/// </summary>
	/// <param name="failCause">MattohaSignal with value of string containing the cause of fail.</param>
	[Signal] public delegate void JoinLobbyFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when setting player data fail.
	/// </summary>
	/// <param name="failCause"></param>
	[Signal] public delegate void SetPlayerDataFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when setting player data fail.
	/// </summary>
	/// <param name="failCause"></param>
	[Signal] public delegate void CreateLobbyFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when setting lobby data fail.
	/// </summary>
	/// <param name="failCause"></param>
	[Signal] public delegate void SetLobbyDataFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when sending team message fail.
	/// </summary>
	/// <param name="failCause"></param>
	[Signal] public delegate void TeamMessageFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when sending lobby message fail.
	/// </summary>
	/// <param name="failCause"></param>
	[Signal] public delegate void LobbyMessageFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when sending global message fail.
	/// </summary>
	/// <param name="failCause"></param>
	[Signal] public delegate void GlobalMessageFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when starting lobby game fail.
	/// </summary>
	/// <param name="failCause"></param>
	[Signal] public delegate void StartGameFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when spawning a node fail.
	/// </summary>
	/// <param name="failCause"></param>
	[Signal] public delegate void SpawnNodeFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when despawning a node fail.
	/// </summary>
	/// <param name="failCause"></param>
	[Signal] public delegate void DespawnNodeFailedEventHandler(MattohaSignal<string> failCause);

	/// <summary>
	/// Emmited when a server called unhandled RPC
	/// </summary>
	/// <param name="method">unhandled method name</param>
	/// <param name="payload"></param>
	[Signal] public delegate void UnhandledClientRpcEventHandler(MattohaSignal<string> method, MattohaSignal<string> payload);
	#endregion


	private MattohaSystem? _sysetm;
	private JsonObject _currentLobbyData = new();
	private JsonObject _currentPlayerData = new();
	private Dictionary<long, JsonObject> _joinedPlayers = new();

	public override void _EnterTree()
	{
		_sysetm = (MattohaSystem)GetNode("..");
		Multiplayer.ConnectedToServer += () => EmitSignal(SignalName.ConnectedToServer);
		Multiplayer.ConnectionFailed += () => EmitSignal(SignalName.ConnectionFailed);
		Multiplayer.ServerDisconnected += () => EmitSignal(SignalName.ServerDisconnected);
		base._EnterTree();
	}


	/// <summary>
	/// Get current player object casted to concrete type instead of JsonObject.
	/// </summary>
	/// <typeparam name="T">type to cast to.</typeparam>
	/// <returns>PlayerObject casted to type T</returns>
	public T? GetCurrentPlayerData<T>() where T : class
	{

		if (typeof(T) == typeof(JsonObject))
		{
			return _currentPlayerData as T;
		}
		return MattohaUtils.Deserialize<T>(_currentPlayerData)!;
	}


	/// <summary>
	/// Get current lobby object casted to concrete type instead of JsonObject.
	/// </summary>
	/// <typeparam name="T">type to cast to.</typeparam>
	/// <returns>LobbyData casted to type T</returns>
	public T? GetCurrentLobbyData<T>() where T : class
	{
		if (typeof(T) == typeof(JsonObject))
		{
			return _currentLobbyData as T;
		}
		return MattohaUtils.Deserialize<T>(_currentLobbyData)!;
	}


	/// <summary>
	/// Return list of joined players casted to concrete type instead of List<JsonObject>.
	/// </summary>
	/// <typeparam name="T">custom type</typeparam>
	/// <returns>list of joined players in same lobby List<T></returns>
	public Dictionary<long, T>? GetJoinedPlayers<T>()
	{
		if (typeof(T) == typeof(JsonObject))
		{
			return _joinedPlayers as Dictionary<long, T>;
		}

		Dictionary<long, T> items = new();
		foreach (var item in _joinedPlayers)
		{
			items.Add(item.Key, MattohaUtils.Deserialize<T>(item.Value)!);
		}
		return items;
	}


	/// <summary>
	/// Connect to server.
	/// </summary>
	public void ConnectToServer()
	{
#if MATTOHA_CLIENT
		if (_sysetm == null)
		{
			GD.Print("SYSTEM IS NULL:");
			return;
		}
		var peer = new ENetMultiplayerPeer();
		peer.CreateClient(_sysetm.DefaultHost, _sysetm.DefaultPort);
		Multiplayer.MultiplayerPeer = peer;
#endif
	}


	/// <summary>
	/// Set or register player data on server, NOTE that: Id, JoindLobbyId cant be set.
	/// </summary>
	/// <param name="player"></param>
	public void SetPlayerData(IMattohaPlayer player)
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.SetPlayerData), MattohaUtils.Serialize(player));
#endif
	}


	/// <summary>
	/// Client Rpc sent by server to client with new player data.
	/// </summary>
	/// <param name="jsonPlayerData">player data as json string.</param>
	private void RpcSetPlayerData(string jsonPlayerData)
	{
#if MATTOHA_CLIENT
		var playerObject = MattohaUtils.ToJsonObject(jsonPlayerData);
		_currentPlayerData = playerObject!;
		EmitSignal(SignalName.CurrentPlayerUpdated, new MattohaSignal<JsonObject> { Value = _currentPlayerData });
#endif
	}


	/// <summary>
	/// Create a new lobby, then join it.
	/// </summary>
	/// <param name="lobby">Lobby object</param>
	public void CreateLobby(IMattohaLobby lobby)
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.CreateLobby), MattohaUtils.Serialize(lobby));
#endif
	}


	/// <summary>
	/// Client RPC sent by server to client with created lobby data.
	/// </summary>
	/// <param name="jsonLobbyData">lobby data as json string.</param>
	private void RpcCreateLobby(string jsonLobbyData)
	{
#if MATTOHA_CLIENT
		var lobbyObject = MattohaUtils.ToJsonObject(jsonLobbyData);
		_currentLobbyData = lobbyObject!;
		EmitSignal(SignalName.NewLobbyCreated, new MattohaSignal<JsonObject> { Value = _currentLobbyData });
#endif
	}


	/// <summary>
	/// Load available lobbies, when done, a signal will be emmited.
	/// </summary>
	public void LoadAvailableLobbies()
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.LoadAvailableLobbies), "");
#endif
	}


	/// <summary>
	/// Client RPC sent by server to client with a list of available lobbies.
	/// </summary>
	private void RpcLoadAvailableLobbies(string jsonListOfAvailableLobbies)
	{
#if MATTOHA_CLIENT
		var availableLobbies = MattohaUtils.Deserialize<List<JsonObject>>(jsonListOfAvailableLobbies);
		EmitSignal(SignalName.AvailableLobbiesRefreshed, new MattohaSignal<List<JsonObject>> { Value = availableLobbies });
#endif
	}


	/// <summary>
	/// Join loby by it's Id.
	/// </summary>
	/// <param name="lobbyId">the lobby ID to join to.</param>
	public void JoinLobby(long lobbyId)
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.JoinLobby), MattohaUtils.Serialize(new { LobbyId = lobbyId }));
#endif
	}


	/// <summary>
	/// Client RPC sent by server when player is joined successfully.
	/// </summary>
	/// <param name="jsonLobbyData">joined lobby data as json string</param>
	public void RpcJoinLobby(string jsonLobbyData)
	{
#if MATTOHA_CLIENT
		_currentLobbyData = MattohaUtils.Deserialize<JsonObject>(jsonLobbyData)!;
		EmitSignal(SignalName.JoinLobbySucceed, new MattohaSignal<JsonObject> { Value = _currentLobbyData });
#endif
	}


	/// <summary>
	/// Client RPC sent by server when a new player joined current lobby.
	/// </summary>
	/// <param name="jsonPlayerData">player data as json string</param>
	public void RpcNewPlayerJoinedLobby(string jsonPlayerData)
	{
#if MATTOHA_CLIENT
		var player = MattohaUtils.Deserialize<JsonObject>(jsonPlayerData);
		EmitSignal(SignalName.NewPlayerJoinedLobby, new MattohaSignal<JsonObject> { Value = player });
#endif
	}


	/// <summary>
	/// Refresh joined lobby data and fetch the data from server.
	/// </summary>
	public void RefreshJoinedLobby()
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.RefreshJoinedLobby), "");
#endif
	}


	/// <summary>
	/// Refresh joined players in lobby.
	/// </summary>
	public void RefreshJoinedPlayers()
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.RefreshJoinedPlayers), "");
#endif
	}


	/// <summary>
	/// Client RPC sent by server with the data if joined lobby. 
	/// </summary>
	/// <param name="jsonLobbyData">lobby data as json string</param>
	public void RpcRefreshJoinedLobby(string jsonLobbyData)
	{
#if MATTOHA_CLIENT
		_currentLobbyData = MattohaUtils.Deserialize<JsonObject>(jsonLobbyData)!;
		EmitSignal(SignalName.JoinedLobbyRefreshed, new MattohaSignal<JsonObject> { Value = _currentLobbyData });
#endif
	}


	/// <summary>
	/// Client RPC sent by server with a dictionary of joined players
	/// </summary>
	/// <param name="jsonPlayersDictionary">Players dictionary</param>
	public void RpcRefreshJoinedPlayers(string jsonPlayersDictionary)
	{
#if MATTOHA_CLIENT
		_joinedPlayers = MattohaUtils.Deserialize<Dictionary<long, JsonObject>>(jsonPlayersDictionary)!;
		EmitSignal(SignalName.JoinedPlayersRefreshed, new MattohaSignal<Dictionary<long, JsonObject>> { Value = _joinedPlayers });
#endif
	}


	/// <summary>
	/// Update Joined Lobby data (only owner can update it), note that Id, PlayersCount & IsGameStarted cant be updated.
	/// </summary>
	/// <param name="lobby"></param>
	public void SetLobbyData(IMattohaLobby lobby)
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.SetLobbyData), MattohaUtils.Serialize(lobby));
#endif
	}


	/// <summary>
	/// Send a message.
	/// </summary>
	/// <param name="message">the message to send</param>
	/// <param name="messageType">MattohaChatMessage: Team, Lobby, Global</param>
	private void SendMessage(string message, string messageType)
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(SendMessage), MattohaUtils.Serialize(new { Message = message, MessageType = messageType }));
#endif
	}


	/// <summary>
	/// Send a team message.
	/// </summary>
	/// <param name="message">the message to send</param>
	public void SendTeamMessage(string message)
	{
#if MATTOHA_CLIENT
		SendMessage(message, nameof(MattohaChatMessage.Team));
#endif
	}


	/// <summary>
	/// Send a lobby message.
	/// </summary>
	/// <param name="message">the message to send</param>
	public void SendLobbyMessage(string message)
	{
#if MATTOHA_CLIENT
		SendMessage(message, nameof(MattohaChatMessage.Lobby));
#endif
	}


	/// <summary>
	/// Send a global message.
	/// </summary>
	/// <param name="message">the message to send</param>
	public void SendGlobalMessage(string message)
	{
#if MATTOHA_CLIENT
		SendMessage(message, nameof(MattohaChatMessage.Global));
#endif
	}


	/// <summary>
	/// Client Rpc sent by client.
	/// </summary>
	/// <param name="jsonPayload">json payload that contains "Message", "MessageType", "PlayerData" keys required for logic.</param>
	private void RpcSendMessage(string jsonPayload)
	{
#if MATTOHA_CLIENT
		var payload = MattohaUtils.ToJsonObject(jsonPayload);
		GD.Print(payload);
		var message = payload!["Message"]!.GetValue<string>();
		var messageType = payload["MessageType"]!.GetValue<string>();
		var playerData = MattohaUtils.Deserialize<JsonObject>(MattohaUtils.Serialize(payload["PlayerData"]!));
		switch (messageType)
		{
			case nameof(MattohaChatMessage.Team):
				EmitSignal(SignalName.TeamMessageReceived, new MattohaSignal<string> { Value = message }, new MattohaSignal<JsonObject> { Value = playerData });
				break;
			case nameof(MattohaChatMessage.Lobby):
				EmitSignal(SignalName.LobbyMessageReceived, new MattohaSignal<string> { Value = message }, new MattohaSignal<JsonObject> { Value = playerData });
				break;
			case nameof(MattohaChatMessage.Global):
				EmitSignal(SignalName.GlobalMessageReceived, new MattohaSignal<string> { Value = message }, new MattohaSignal<JsonObject> { Value = playerData });
				break;
		}
#endif
	}


	/// <summary>
	/// Start joined lobby game, only owner can start games.
	/// </summary>
	public void StartGame()
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.StartGame), "");
#endif
	}


	/// <summary>
	/// Client RPC sent by Server to notify players that a game is started.
	/// </summary>
	private void RpcStartGame()
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.GameStarted);
#endif
	}


	/// <summary>
	/// Leave joined lobby
	/// </summary>
	public void LeaveLobby()
	{
#if MATTOHA_CLIENT
		_currentLobbyData = new();
		_currentPlayerData[nameof(IMattohaPlayer.JoinedLobbyId)] = 0;
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.LeaveLobby), "");
#endif
	}


	/// <summary>
	/// Clitn RPC sent by server to notify players that a player left.
	/// </summary>
	/// <param name="jsonPlayerData">player data as json string</param>
	private void RpcPlayerLeft(string jsonPlayerData)
	{
#if MATTOHA_CLIENT
		var playerData = MattohaUtils.Deserialize<JsonObject>(jsonPlayerData);
		EmitSignal(SignalName.PlayerLeftLobby, new MattohaSignal<JsonObject> { Value = playerData });
#endif
	}


	/// <summary>
	/// Spawn a node for players in joined lobby.
	/// </summary>
	/// <param name="node">Node to spawn (After Added To Tree)</param>
	public void SpawnNode(Node node)
	{
#if MATTOHA_CLIENT
		var info = new MattohaSpawnNodeInfo()
		{
			NodeName = node.Name,
			ParentPath = node.GetParent().GetPath().ToString(),
			SceneFile = node.SceneFilePath,
			OwnerId = Multiplayer.GetUniqueId(),
			Position = node.Get("position"),
			Rotation = node.Get("rotation")
		};

		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.SpawnNode), MattohaUtils.Serialize(info));
#endif
	}


	/// <summary>
	/// Client RPC sent by server to tell client to spawn a node.
	/// </summary>
	/// <param name="jsonSpawnNodeInfo"></param>
	private void RpcSpawnNode(string jsonSpawnNodeInfo)
	{
#if MATTOHA_CLIENT
		EmitSignal(
			SignalName.NodeSpawnRequested,
			new MattohaSignal<MattohaSpawnNodeInfo> { Value = MattohaUtils.Deserialize<MattohaSpawnNodeInfo>(jsonSpawnNodeInfo) }
		);
#endif
	}


	/// <summary>
	/// Spawn nodes that already spawned from other players, use this method when a new player join the game scene.
	/// </summary>
	public void SpawnAvailableNodes()
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.SpawnAvailableNodes), "");
#endif
	}


	/// <summary>
	/// Despawn node on players's devices, player can despawn nodes he owns only.
	/// </summary>
	public void DespawnNode(Node node)
	{
#if MATTOHA_CLIENT

		var info = new MattohaSpawnNodeInfo()
		{
			NodeName = node.Name,
			ParentPath = node.GetParent().GetPath().ToString(),
			SceneFile = node.SceneFilePath,
			OwnerId = Multiplayer.GetUniqueId(),
		};
		RpcId(1, nameof(ServerRpc), nameof(MattohaServerRpcMethods.DespawnNode), MattohaUtils.Serialize(info));
#endif
	}


	/// <summary>
	/// Client RPC sent by server to despawn node by its path.
	/// </summary>
	/// <param name="nodePath"></param>
	private void RpcDespawnNode(string nodePath)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.NodeDespawnRequested, new MattohaSignal<string> { Value = nodePath });
#endif
	}


	/// <summary>
	/// Send a custom method name RPC to server to execute, this will emmit "UnhandledRpc" signal on server node.
	/// </summary>
	/// <param name="methodName">method name to handle.</param>
	/// <param name="paylaod">payload sent.</param>
	public void SendUnhandledServerRpc(string methodName, string paylaod)
	{
#if MATTOHA_CLIENT
		RpcId(1, nameof(ServerRpc), methodName, paylaod);
#endif

	}


	public void ClientRpc(string method, string payload)
	{
#if MATTOHA_CLIENT
		switch (method)
		{
			case nameof(MattohaClientRpcMethods.SetPlayerData):
				RpcSetPlayerData(payload);
				break;
			case nameof(MattohaClientRpcMethods.CreateLobby):
				RpcCreateLobby(payload);
				break;
			case nameof(MattohaClientRpcMethods.LoadAvailableLobbies):
				RpcLoadAvailableLobbies(payload);
				break;
			case nameof(MattohaClientRpcMethods.JoinLobby):
				RpcJoinLobby(payload);
				break;
			case nameof(MattohaClientRpcMethods.NewPlayerJoinedLobby):
				RpcNewPlayerJoinedLobby(payload);
				break;
			case nameof(MattohaClientRpcMethods.RefreshJoinedLobby):
				RpcRefreshJoinedLobby(payload);
				break;
			case nameof(MattohaClientRpcMethods.RefreshJoinedPlayers):
				RpcRefreshJoinedPlayers(payload);
				break;
			case nameof(MattohaClientRpcMethods.SendMessage):
				RpcSendMessage(payload);
				break;
			case nameof(MattohaClientRpcMethods.StartGame):
				RpcStartGame();
				break;
			case nameof(MattohaClientRpcMethods.PlayerLeft):
				RpcPlayerLeft(payload);
				break;
			case nameof(MattohaClientRpcMethods.SpawnNode):
				RpcSpawnNode(payload);
				break;
			case nameof(MattohaClientRpcMethods.DespawnNode):
				RpcDespawnNode(payload);
				break;
			default:
				GD.Print("Unhandled Client RPC: " + method);
				EmitSignal(SignalName.UnhandledClientRpc, new MattohaSignal<string> { Value = method }, new MattohaSignal<string> { Value = payload });
				break;
		}
#endif
	}


	public void ClientRpcFail(string mattohaFailType, string failCause)
	{
#if MATTOHA_CLIENT
		switch (mattohaFailType)
		{
			case nameof(MattohaFailType.CreateLobby):
				EmitSignal(SignalName.CreateLobbyFailed, new MattohaSignal<string> { Value = failCause });
				break;
			case nameof(MattohaFailType.SendLobbyMessage):
				EmitSignal(SignalName.LobbyMessageFailed, new MattohaSignal<string> { Value = failCause });
				break;
			case nameof(MattohaFailType.SendTeamMessage):
				EmitSignal(SignalName.TeamMessageFailed, new MattohaSignal<string> { Value = failCause });
				break;
			case nameof(MattohaFailType.SendGlobalMessage):
				EmitSignal(SignalName.GlobalMessageFailed, new MattohaSignal<string> { Value = failCause });
				break;
			case nameof(MattohaFailType.JoinLobby):
				EmitSignal(SignalName.JoinLobbyFailed, new MattohaSignal<string> { Value = failCause });
				break;
			case nameof(MattohaFailType.SetLobbyData):
				EmitSignal(SignalName.SetLobbyDataFailed, new MattohaSignal<string> { Value = failCause });
				break;
			case nameof(MattohaFailType.SetPlayerData):
				EmitSignal(SignalName.SetPlayerDataFailed, new MattohaSignal<string> { Value = failCause });
				break;
			case nameof(MattohaFailType.StartGame):
				EmitSignal(SignalName.StartGameFailed, new MattohaSignal<string> { Value = failCause });
				break;
			case nameof(MattohaFailType.SpawnNode):
				EmitSignal(SignalName.SpawnNodeFailed, new MattohaSignal<string> { Value = failCause });
				break;
			case nameof(MattohaFailType.DespawnNode):
				EmitSignal(SignalName.DespawnNodeFailed, new MattohaSignal<string> { Value = failCause });
				break;
		}
#endif
	}


	/// <summary>
	/// Server Rpc sent by client with method name to execute on server.
	/// </summary>
	/// <param name="method">method name, one of MattohaServerRpcMethods enum.</param>
	/// <param name="jsonPayload">the payload required to execute the logic.</param>
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ServerRpc(string method, string jsonPayload)
	{
#if MATTOHA_SERVER
		_sysetm?.Server?.ServerRpc(method, jsonPayload);
#endif
	}

}
