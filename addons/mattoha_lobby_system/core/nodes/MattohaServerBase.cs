using Godot;
using MattohaLobbySystem.Core.Enums;
using MattohaLobbySystem.Core.Interfaces;
using MattohaLobbySystem.Core.Models;
using MattohaLobbySystem.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace MattohaLobbySystem.Core.Nodes;

public partial class MattohaServerBase : Node, IMattohaClientRpc, IMattohaServerRpc
{

	/// <summary>
	/// Emmited when a client called unhandled RPC
	/// </summary>
	/// <param name="method">unhandled method name</param>
	/// <param name="payload"></param>
	[Signal] public delegate void UnhandledServerRpcEventHandler(MattohaSignal<string> method, MattohaSignal<string> payload);


	[Export] MattohaServerMiddleware? Middleware { get; set; }
	private MattohaSystem? _sysetm;

	private readonly Dictionary<long, JsonObject> _players = new();
	private readonly Dictionary<long, JsonObject> _lobbies = new();

	/// <summary>
	/// Spawned nodes in each lobby, every key has a value of List<MattohaSpawnNodeInfo>.
	/// </summary>
	public Dictionary<long, List<MattohaSpawnNodeInfo>> SpawnedNodes { get; private set; } = new();

	public override void _EnterTree()
	{
		_sysetm = (MattohaSystem)GetNode("..");
		Middleware ??= new();
		Multiplayer.PeerDisconnected += OnPeerDisconnected;
		base._EnterTree();
	}


	/// <summary>
	/// executes when any peer disconnected.
	/// </summary>
	/// <param name="id"></param>
	private void OnPeerDisconnected(long id)
	{
#if MATTOHA_SERVER
		if (Multiplayer.IsServer())
		{
			DespawnPlayerNodes(id);
			RemovePlayerFromJoinedLobby(id);
		}
#endif
	}


	/// <summary>
	/// Get Players dictionary, but cast it to custom concrete objects instead of JsonObject,
	/// where key is the id of player, and the value is the player object.
	/// </summary>
	/// <typeparam name="T">Csutom type to cast to.</typeparam>
	/// <returns>Dictionary of players {Id => PlayerObject} </returns>
	public Dictionary<long, T>? GetPlayers<T>()
	{
		if (typeof(T) == typeof(JsonObject))
		{
			return _players as Dictionary<long, T>;
		}

		Dictionary<long, T> items = new();
		foreach (var kvp in _players)
		{
			items.Add(kvp.Key, MattohaUtils.Deserialize<T>(kvp.Value)!);
		}
		return items;
	}


	/// <summary>
	/// Get lobbies dictionary, but cast it to custom concrete objects instead of JsonObject,
	/// where key is the id of lobby, and the value is the lobby object.
	/// </summary>
	/// <typeparam name="T">Csutom type to cast to.</typeparam>
	/// <returns>Dictionary of lobbies {Id => LobbyObject} </returns>
	public Dictionary<long, T>? GetLobbies<T>()
	{
		if (typeof(T) == typeof(JsonObject))
		{
			return _lobbies as Dictionary<long, T>;
		}
		Dictionary<long, T> items = new();
		foreach (var kvp in _lobbies)
		{
			items.Add(kvp.Key, MattohaUtils.Deserialize<T>(kvp.Value)!);
		}
		return items;
	}


	/// <summary>
	/// Get Player from players dictionary but casted to custom type.
	/// </summary>
	/// <param name="playerId"></param>
	/// <returns>null if not exists</returns>
	public T? GetPlayer<T>(long playerId) where T : class
	{
		if (_players.Keys.Contains(playerId))
		{
			if (typeof(T) == typeof(JsonObject))
			{
				return _players[playerId] as T;
			}
			return MattohaUtils.Deserialize<T>(_players[playerId])!;
		}
		return default;
	}


	/// <summary>
	/// Get Lobby from lobbies dictionary but casted to custom type.
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns>null if not exists</returns>
	public T? GetLobby<T>(long lobbyId) where T : class
	{
		if (_lobbies.Keys.Contains(lobbyId))
		{
			if (typeof(T) == typeof(JsonObject))
			{
				return _lobbies[lobbyId] as T;
			}
			return MattohaUtils.Deserialize<T>(_players[lobbyId])!;
		}
		return default;
	}


	/// <summary>
	/// Get List of players joined in lobby.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public List<T>? GetPlayersInLobby<T>(long lobbyId) where T : class
	{
		var players = _players.Values.Where(pl => pl[nameof(IMattohaPlayer.JoinedLobbyId)]!.GetValue<long>() == lobbyId).ToList();
		if (typeof(T) == typeof(JsonObject))
		{
			return players as List<T>;
		}
		return players.Select(pl => MattohaUtils.Deserialize<T>(pl)).ToList() as List<T>;

	}


	/// <summary>
	/// Get the joined lobby from player id.
	/// </summary>
	/// <param name="playerId"> player id to get his joined lobby</param>
	/// <returns>Lobby that player is joined to.</returns>
	public T? GetLobbyOfPlayer<T>(long playerId) where T : class
	{
		var player = GetPlayer<JsonObject>(playerId);
		if (player == null)
			return null;
		var lobbyId = player[nameof(IMattohaPlayer.JoinedLobbyId)]!.GetValue<long>();
		var lobby = GetLobby<JsonObject>(lobbyId);
		if (lobby == null)
			return null;
		if (typeof(T) == typeof(JsonObject))
		{
			return lobby as T;
		}

		return MattohaUtils.Deserialize<T>(lobby);
	}


	/// <summary>
	/// Start the server session.
	/// </summary>
	public void StartServer()
	{
#if MATTOHA_SERVER
		if (_sysetm == null)
		{
			return;
		}
		var peer = new ENetMultiplayerPeer();
		peer.CreateServer(_sysetm.DefaultPort, _sysetm.MaxPlayersInServer, _sysetm.MaxRpcChannels);
		Multiplayer.MultiplayerPeer = peer;
#endif
	}


	/// <summary>
	/// Server RPC sent by client to set or register player data.
	/// </summary>
	/// <param name="jsonPlayerData">Player object as json string, should implements IMattohaPlayer.</param>
	private void RpcSetPlayerData(string jsonPlayerData)
	{
#if MATTOHA_SERVER
		var playerId = (long)Multiplayer.GetRemoteSenderId();
		var playerData = MattohaUtils.ToJsonObject(jsonPlayerData);
		playerData![nameof(IMattohaPlayer.JoinedLobbyId)] = (long)0;
		var player = GetPlayer<JsonObject>(playerId);

		playerData![nameof(IMattohaPlayer.Id)] = playerId;
		if (player != null)
		{
			playerData![nameof(IMattohaPlayer.JoinedLobbyId)] = player[nameof(IMattohaPlayer.JoinedLobbyId)]!.GetValue<long>();
		}
		var resp = Middleware!.BeforeSetPlayerData(this, playerData);
		if (resp.Status)
		{
			if (player == null)
			{
				_players.Add(playerId, playerData);
			}
			else
			{
				_players[playerId] = playerData;
			}
			player = GetPlayer<JsonObject>(playerId);
			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.SetPlayerData), MattohaUtils.Serialize(player!));

			// todo: optimize the process, send only the player data and then client should refresh existsing player.
			// refresh players list for all joined players in lobby
			var joinedLobbyId = player![nameof(IMattohaPlayer.JoinedLobbyId)]!.GetValue<long>();
			if (joinedLobbyId != 0)
			{
				RefreshJoinedPlayersForAll(joinedLobbyId);
			}
			Middleware!.AfterSetPlayerData(this, playerData);
		}
		else
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.SetPlayerData), resp.Message);
		}
#endif
	}


	/// <summary>
	/// Generate a lobby id.
	/// </summary>
	/// <returns>unique lobby id</returns>
	private long GenerateLobbyId()
	{
		long newId;
		if (_lobbies.Count == 0)
		{
			newId = GD.Randi() % 100;
		}
		else
		{
			newId = _lobbies.Keys.Max() + (GD.Randi() % 100);
		}

		if (_lobbies.Keys.Contains(newId))
		{
			newId = GenerateLobbyId();
		}
		return newId;
	}


	/// <summary>
	/// Server RPC sent by client to Create new lobby.
	/// </summary>
	/// <param name="jsonLobbyData">Lobby data as json string, should implements IMattohaLobby.</param>
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void RpcCreateLobby(string jsonLobbyData)
	{
#if MATTOHA_SERVER
		var playerId = (long)Multiplayer.GetRemoteSenderId();
		var player = GetPlayer<JsonObject>(playerId);
		if (player == null)
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.CreateLobby), "invalid-player");
			return;
		}
		var lobbyData = MattohaUtils.ToJsonObject(jsonLobbyData);
		if (lobbyData![nameof(IMattohaLobby.MaxPlayers)]!.GetValue<long>() > _sysetm!.MaxPlayersPerLobby)
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.CreateLobby), $"invalid-max-players,{_sysetm!.MaxPlayersPerLobby}");
			return;
		}

		var resp = Middleware!.BeforeCreateLobby(this, lobbyData);
		if (resp.Status)
		{
			lobbyData![nameof(IMattohaLobby.Id)] = GenerateLobbyId();
			player[nameof(IMattohaPlayer.JoinedLobbyId)] = MattohaUtils.ToJsonNode(lobbyData[nameof(IMattohaLobby.Id)]!);
			lobbyData[nameof(IMattohaLobby.PlayersCount)] = 1;
			lobbyData[nameof(IMattohaLobby.OwnerId)] = playerId;
			_lobbies.Add(lobbyData[nameof(IMattohaLobby.Id)]!.GetValue<long>(), lobbyData);

			SpawnedNodes.Add(lobbyData[nameof(IMattohaLobby.Id)]!.GetValue<long>(), new());

			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.SetPlayerData), MattohaUtils.Serialize(player!));
			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.CreateLobby), MattohaUtils.Serialize(lobbyData!));
			if (_sysetm?.AutoRefreshAvailableLobbies == true)
			{
				RefreshAvailableLobbiesForAll();
			}
		}
		else
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.CreateLobby), resp.Message);
		}
#endif
	}


	/// <summary>
	/// Send available lobbies for all players.
	/// </summary>
	public void RefreshAvailableLobbiesForAll()
	{
#if MATTOHA_SERVER
		var lobbiesWithoutPrivateProps = _lobbies.Values.Select(lobby => MattohaUtils.ToHiddenPrivatePropsObject(lobby)).ToList();
		foreach (var playerId in _players.Keys)
		{
			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.LoadAvailableLobbies), MattohaUtils.Serialize(lobbiesWithoutPrivateProps));
		}
#endif
	}


	/// <summary>
	/// Server RPC sent by client to request a list of available lobbies.
	/// </summary>
	private void RpcLoadAvailableLobbies()
	{
#if MATTOHA_SERVER
		var playerId = (long)Multiplayer.GetRemoteSenderId();
		var player = GetPlayer<JsonObject>(playerId);
		if (player != null)
		{
			var lobbiesWithoutPrivateProps = _lobbies.Values.Select(lobby => MattohaUtils.ToHiddenPrivatePropsObject(lobby)).ToList();
			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.LoadAvailableLobbies), MattohaUtils.Serialize(lobbiesWithoutPrivateProps));
		}
#endif
	}


	/// <summary>
	/// Server RPC sent by client to join an existing lobby.
	/// </summary>
	/// <param name="jsonPayload">Payload that has "LobbyId" key to join</param>
	private void RpcJoinLobby(string jsonPayload)
	{
		var jsonObject = MattohaUtils.Deserialize<JsonObject>(jsonPayload);
		long lobbyId = jsonObject!["LobbyId"]!.GetValue<long>();
#if MATTOHA_SERVER
		var lobby = GetLobby<JsonObject>(lobbyId);
		var playerId = (long)Multiplayer.GetRemoteSenderId();
		if (lobby == null)
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.JoinLobby), "lobby-not-found");
			return;
		}
		var player = GetPlayer<JsonObject>(playerId);
		if (player == null)
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.JoinLobby), "player-not-found");
			return;
		}
		if (lobby[nameof(IMattohaLobby.PlayersCount)]!.GetValue<int>() >= lobby[nameof(IMattohaLobby.MaxPlayers)]!.GetValue<int>())
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.JoinLobby), "max-players-reached");
			return;
		}
		var resp = Middleware!.BeforeJoinLobby(this, player, lobby);
		if (resp.Status)
		{
			RemovePlayerFromJoinedLobby(playerId);
			player[nameof(IMattohaPlayer.JoinedLobbyId)] = MattohaUtils.ToJsonNode(lobby[nameof(IMattohaLobby.Id)]!);
			lobby[nameof(IMattohaLobby.PlayersCount)] = lobby[nameof(IMattohaLobby.PlayersCount)]!.GetValue<int>() + 1;
			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.SetPlayerData), MattohaUtils.Serialize(player!));
			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.JoinLobby), MattohaUtils.Serialize(MattohaUtils.ToHiddenPrivatePropsObject(lobby)!));
			RefreshAvailableLobbiesForAll();
			// all players in joined lobby should be notified that:
			// - a new player joined
			// - a lobby data is updated
			// - a players list is updated
			var joiendPlayers = GetPlayersInLobby<JsonObject>(lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>());
			foreach (var pl in joiendPlayers!)
			{
				var joinedPlayerId = pl[nameof(IMattohaLobby.Id)]!.GetValue<long>()!;
				if (joinedPlayerId != playerId)
				{
					RpcId(joinedPlayerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.NewPlayerJoinedLobby), MattohaUtils.Serialize(MattohaUtils.ToHiddenPrivatePropsObject(player)));
				}
				RefreshJoinedLobbyForPlayer(pl[nameof(IMattohaLobby.Id)]!.GetValue<long>()!);
				RefreshJoinedPlayersForPlayer(joinedPlayerId);
			}
			Middleware!.AfterJoinLobby(this, player, lobby);
		}
		else
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.JoinLobby), resp.Message);
		}
#endif
	}


	/// <summary>
	/// Server RPC sent by client to refresh current joined lobby data.
	/// </summary>
	private void RpcRefreshLobbyData()
	{
#if MATTOHA_SERVER
		var playerId = (long)Multiplayer.GetRemoteSenderId();
		RefreshJoinedLobbyForPlayer(playerId);
#endif
	}


	/// <summary>
	/// Refresh joined lobby for a specific player.
	/// </summary>
	/// <param name="playerId">player id to send the RPC to</param>
	public void RefreshJoinedLobbyForPlayer(long playerId)
	{
#if MATTOHA_SERVER
		var lobby = GetLobbyOfPlayer<JsonObject>(playerId);
		if (lobby == null)
			return;
		var ownerId = lobby[nameof(IMattohaLobby.OwnerId)]!.GetValue<long>();
		if (ownerId == playerId)
		{
			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.RefreshJoinedLobby), MattohaUtils.Serialize(lobby));
		}
		else
		{
			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.RefreshJoinedLobby), MattohaUtils.Serialize(MattohaUtils.ToHiddenPrivatePropsObject(lobby)!));
		}
#endif
	}


	/// <summary>
	/// Refresh joined lobby for all players joined.
	/// </summary>
	/// <param name="lobbyId">the id of the target lobby</param>
	public void RefreshJoinedLobbyForAll(long lobbyId)
	{
#if MATTOHA_SERVER
		var lobby = GetLobby<JsonObject>(lobbyId);
		if (lobby == null)
			return;
		var players = GetPlayersInLobby<JsonObject>(lobbyId);
		var ownerId = lobby[nameof(IMattohaLobby.OwnerId)]!.GetValue<long>();
		foreach (var player in players!)
		{
			var playerId = player[nameof(IMattohaPlayer.Id)]!.GetValue<long>();
			if (ownerId == playerId)
			{
				RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.RefreshJoinedLobby), MattohaUtils.Serialize(lobby));
			}
			else
			{
				RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.RefreshJoinedLobby), MattohaUtils.Serialize(MattohaUtils.ToHiddenPrivatePropsObject(lobby)!));
			}
		}
#endif
	}


	/// <summary>
	/// Server RPC sent by client to refresh joined players list.
	/// </summary>
	private void RpcRefreshJoinedPlayers()
	{
#if MATTOHA_SERVER
		RefreshJoinedPlayersForPlayer(Multiplayer.GetRemoteSenderId());
#endif
	}


	/// <summary>
	/// Send the joined players list to a specific player.
	/// </summary>
	/// <param name="playerId">player id to send the RPC to.</param>
	public void RefreshJoinedPlayersForPlayer(long playerId)
	{
#if MATTOHA_SERVER
		var lobby = GetLobbyOfPlayer<JsonObject>(playerId);
		if (lobby == null)
			return;

		var players = GetPlayersInLobby<JsonObject>(lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>());
		var playersWithoutPrivateProps = players!.Select(p => MattohaUtils.ToHiddenPrivatePropsObject(p)).ToList();
		var playersDict = playersWithoutPrivateProps!.ToDictionary(p => p[nameof(IMattohaPlayer.Id)]!.GetValue<long>(), p => p);
		RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.RefreshJoinedPlayers), MattohaUtils.Serialize(playersDict));
#endif
	}


	/// <summary>
	/// Send the joined players list to a all players in specific lobby.
	/// </summary>
	/// <param name="lobbyId">lobby id.</param>
	public void RefreshJoinedPlayersForAll(long lobbyId)
	{
#if MATTOHA_SERVER
		var lobby = GetLobby<JsonObject>(lobbyId);
		if (lobby == null)
			return;
		var players = GetPlayersInLobby<JsonObject>(lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>());
		var playersWithoutPrivateProps = players!.Select(p => MattohaUtils.ToHiddenPrivatePropsObject(p)).ToList();
		var playersDict = playersWithoutPrivateProps!.ToDictionary(p => p[nameof(IMattohaPlayer.Id)]!.GetValue<long>(), p => p);
		foreach (var joinedPlayer in players!)
		{
			RpcId(
				joinedPlayer[nameof(IMattohaPlayer.Id)]!.GetValue<long>(),
				nameof(ClientRpc), nameof(MattohaClientRpcMethods.RefreshJoinedPlayers),
				MattohaUtils.Serialize(playersDict)
			);
		}
#endif
	}


	/// <summary>
	/// Refresh Lobby data for all joined players in the lobby.
	/// </summary>
	/// <param name="lobbyId"></param>
	public void RefreshLobbyDataForAllJoinedPlayers(long lobbyId)
	{
#if MATTOHA_SERVER
		var lobby = GetLobby<JsonObject>(lobbyId);
		if (lobby == null)
			return;
		var players = GetPlayersInLobby<JsonObject>(lobbyId);
		foreach (var pl in players!)
		{
			RefreshJoinedLobbyForPlayer(pl[nameof(IMattohaPlayer.Id)]!.GetValue<long>());
		}
#endif

	}


	/// <summary>
	/// Server RPC sent by client to update lobby data
	/// </summary>
	/// <param name="jsonLobbyData">Lobby data as json string, should implements IMattohaLobby.</param>
	private void RpcSetLobbyData(string jsonLobbyData)
	{
#if MATTOHA_SERVER
		var playerId = (long)Multiplayer.GetRemoteSenderId();
		var player = GetPlayer<JsonObject>(playerId);
		if (player == null)
			return;
		var lobby = GetLobbyOfPlayer<JsonObject>(playerId);
		if (lobby == null)
			return;
		var lobbyOwnerId = lobby[nameof(IMattohaLobby.OwnerId)]!.GetValue<long>();
		if (lobbyOwnerId != playerId)
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.SetLobbyData), "not-owner");
			return;
		}
		var lobbyData = MattohaUtils.ToJsonObject(jsonLobbyData);
		var playersCount = lobby[nameof(IMattohaLobby.PlayersCount)]!.GetValue<int>();
		var isGameStarted = lobby[nameof(IMattohaLobby.IsGameStarted)]!.GetValue<bool>();
		lobbyData![nameof(IMattohaLobby.PlayersCount)] = playersCount;
		lobbyData![nameof(IMattohaLobby.IsGameStarted)] = isGameStarted;

		var resp = Middleware!.BeforeSetLobbyData(this, player, lobbyData!);

		if (resp.Status)
		{
			// reset players acount & is Game started incase if the middleware changed it.
			lobbyData![nameof(IMattohaLobby.PlayersCount)] = playersCount;
			lobbyData![nameof(IMattohaLobby.IsGameStarted)] = isGameStarted;
			var lobbyId = lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>();

			_lobbies[lobbyId] = lobbyData;
			RefreshLobbyDataForAllJoinedPlayers(lobbyId);
			Middleware!.AfterSetLobbyData(this, player, lobbyData!);
		}
		else
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.SetLobbyData), resp.Message);
		}
#endif
	}


	/// <summary>
	/// Server RPC sent by client to send a messgage.
	/// </summary>
	/// <param name="jsonPayload">Json payload that contains "Message" & "MessageType".</param>
	private void RpcSendMessage(string jsonPayload)
	{
#if MATTOHA_SERVER
		var payload = MattohaUtils.Deserialize<JsonObject>(jsonPayload);
		var message = payload!["Message"]!.GetValue<string>();
		var messageType = payload!["MessageType"]!.GetValue<string>();
		switch (messageType)
		{
			case nameof(MattohaChatMessage.Team):
				SendTeamMessage(message, Multiplayer.GetRemoteSenderId());
				break;
			case nameof(MattohaChatMessage.Lobby):
				SendLobbyMessage(message, Multiplayer.GetRemoteSenderId());
				break;
			case nameof(MattohaChatMessage.Global):
				SendGlobalMessage(message, Multiplayer.GetRemoteSenderId());
				break;
			default:
				SendGlobalMessage(message, Multiplayer.GetRemoteSenderId());
				break;
		}
#endif
	}


	/// <summary>
	/// Send a message for all players in same lobby and same team.
	/// </summary>
	/// <param name="message">The message content.</param>
	/// <param name="fromPlayerId">player id who is sending a message.</param>
	public void SendTeamMessage(string message, long fromPlayerId)
	{
		var joinedLobby = GetLobbyOfPlayer<JsonObject>(fromPlayerId);
		if (joinedLobby == null)
			return;
		var fromPlayer = GetPlayer<JsonObject>(fromPlayerId);

		var resp = Middleware!.BeforeSendTeamMessage(this, fromPlayer!);
		if (resp.Status)
		{
			var players = GetPlayersInLobby<JsonObject>(joinedLobby[nameof(IMattohaLobby.Id)]!.GetValue<long>());
			foreach (var player in players!)
			{
				if (player[nameof(IMattohaPlayer.TeamId)]!.GetValue<long>() == fromPlayer![nameof(IMattohaPlayer.TeamId)]!.GetValue<long>())
				{
					RpcId(
						player[nameof(IMattohaPlayer.Id)]!.GetValue<long>(),
						nameof(ClientRpc),
						nameof(MattohaClientRpcMethods.SendMessage),
						MattohaUtils.Serialize(new
						{
							Message = message,
							MessageType = nameof(MattohaChatMessage.Team),
							PlayerData = MattohaUtils.ToChatObject(fromPlayer!)
						})
					);
				}
			}
			Middleware!.AfterSendTeamMessage(this, fromPlayer!);
		}
		else
		{
			RpcId(fromPlayerId, nameof(ClientRpcFail), nameof(MattohaFailType.SendTeamMessage), resp.Message);
		}
	}


	/// <summary>
	/// Send a message for all players in same lobby.
	/// </summary>
	/// <param name="message">The message content.</param>
	/// <param name="fromPlayerId">player id who is sending a message.</param>
	public void SendLobbyMessage(string message, long fromPlayerId)
	{
		var joinedLobby = GetLobbyOfPlayer<JsonObject>(fromPlayerId);
		if (joinedLobby == null)
			return;
		var fromPlayer = GetPlayer<JsonObject>(fromPlayerId);
		var resp = Middleware!.BeforeSendLobbyMessage(this, fromPlayer!, joinedLobby!);
		if (resp.Status)
		{
			var players = GetPlayersInLobby<JsonObject>(joinedLobby[nameof(IMattohaLobby.Id)]!.GetValue<long>());
			foreach (var player in players!)
			{
				RpcId(
					player[nameof(IMattohaPlayer.Id)]!.GetValue<long>(),
					nameof(ClientRpc),
					nameof(MattohaClientRpcMethods.SendMessage),
					MattohaUtils.Serialize(new
					{
						Message = message,
						MessageType = nameof(MattohaChatMessage.Lobby),
						PlayerData = MattohaUtils.ToChatObject(fromPlayer!)
					})
				);
			}
			Middleware!.AfterSendLobbyMessage(this, fromPlayer!, joinedLobby!);
		}
		else
		{
			RpcId(fromPlayerId, nameof(ClientRpcFail), nameof(MattohaFailType.SendLobbyMessage), resp.Message);
		}
	}


	/// <summary>
	/// Send a message for all online players.
	/// </summary>
	/// <param name="message">The message content.</param>
	/// <param name="fromPlayerId">player id who is sending a message.</param>
	public void SendGlobalMessage(string message, long fromPlayerId)
	{
		var fromPlayer = GetPlayer<JsonObject>(fromPlayerId);
		if (fromPlayer == null)
			return;

		var resp = Middleware!.BeforeSendGlobalMessage(this, fromPlayer!);
		if (resp.Status)
		{
			foreach (var player in _players.Values)
			{
				RpcId(
					player[nameof(IMattohaPlayer.Id)]!.GetValue<long>(),
					nameof(ClientRpc),
					nameof(MattohaClientRpcMethods.SendMessage),
					MattohaUtils.Serialize(new
					{
						Message = message,
						MessageType = nameof(MattohaChatMessage.Global),
						PlayerData = MattohaUtils.ToChatObject(fromPlayer!)
					})
				);
			}
			Middleware!.AfterSendGlobalMessage(this, fromPlayer!);
		}
		else
		{
			RpcId(fromPlayerId, nameof(ClientRpcFail), nameof(MattohaFailType.SendGlobalMessage), resp.Message);
		}
	}


	/// <summary>
	/// Server RPC sent by client to start owning lobby.
	/// </summary>
	private void RpcStartGame()
	{
#if MATTOHA_SERVER
		var playerId = (long)Multiplayer.GetRemoteSenderId();
		var lobby = GetLobbyOfPlayer<JsonObject>(playerId);
		if (lobby == null)
			return;
		if (lobby[nameof(IMattohaLobby.OwnerId)]!.GetValue<long>() != playerId)
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.StartGame), "not-owner");
			return;
		}
		var owner = GetPlayer<JsonObject>(playerId);
		var resp = Middleware!.BeforeStartGame(this, owner!, lobby);
		if (resp.Status)
		{
			lobby[nameof(IMattohaLobby.IsGameStarted)] = true;
			var lobbyPlayers = GetPlayersInLobby<JsonObject>(lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>());
			foreach (var joinedPlayer in lobbyPlayers!)
			{
				RpcId(joinedPlayer[nameof(IMattohaPlayer.Id)]!.GetValue<long>(), nameof(ClientRpc), nameof(MattohaClientRpcMethods.StartGame), "");
			}
			if (_sysetm?.AutoRefreshAvailableLobbies == true)
			{
				RefreshAvailableLobbiesForAll();
			}
			RefreshJoinedLobbyForAll(lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>());
			Middleware!.AfterStartGame(this, owner!, lobby);
		}
		else
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.StartGame), resp.Message);
		}
#endif
	}


	/// <summary>
	/// Despawn player nodes from his current joined lobby.
	/// </summary>
	/// <param name="playerId"></param>
	public void DespawnPlayerNodes(long playerId)
	{
#if MATTOHA_SERVER
		var lobby = GetLobbyOfPlayer<JsonObject>(playerId);
		if (lobby == null)
			return;
		var lobbyId = lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>();
		var players = GetPlayersInLobby<JsonObject>(lobbyId);
		var playerNodes = SpawnedNodes[lobbyId].FindAll(node => node.OwnerId == playerId);
		foreach (var playerNode in playerNodes)
		{
			foreach (var player in players!)
			{
				var joinedPlayerId = player[nameof(IMattohaPlayer.Id)]!.GetValue<long>();
				if (playerId != joinedPlayerId)
				{
					RpcId(joinedPlayerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.DespawnNode), $"{playerNode.ParentPath}/{playerNode!.NodeName}");
				}
			}
			SpawnedNodes[lobbyId].Remove(playerNode);
		}
#endif
	}


	/// <summary>
	/// Remove player from joined lobby.
	/// </summary>
	/// <param name="playerId"></param>
	public void RemovePlayerFromJoinedLobby(long playerId)
	{
#if MATTOHA_SERVER
		var player = GetPlayer<JsonObject>(playerId);
		if (player == null)
			return;
		var lobby = GetLobbyOfPlayer<JsonObject>(playerId);
		if (lobby == null)
			return;
		var lobbyId = lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>();
		Middleware!.BeforePlayerLeaveLobby(this, player, lobby);
		player[nameof(IMattohaPlayer.JoinedLobbyId)] = (long)0;
		lobby[nameof(IMattohaLobby.PlayersCount)] = lobby[nameof(IMattohaLobby.PlayersCount)]!.GetValue<int>() - 1;
		// check if lobby is empty then remove it, otherwise , change owner if the left player is the owner
		if (lobby[nameof(IMattohaLobby.PlayersCount)]!.GetValue<int>() == 0)
		{
			_lobbies.Remove(lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>());
		}
		else
		{
			var joinedPlayers = GetPlayersInLobby<JsonObject>(lobbyId);
			if (lobby[nameof(IMattohaLobby.OwnerId)]!.GetValue<long>() == playerId)
			{
				lobby[nameof(IMattohaLobby.OwnerId)] = joinedPlayers!.First()[nameof(IMattohaPlayer.Id)]!.GetValue<long>();
			}
			RefreshJoinedLobbyForAll(lobbyId);
			RefreshJoinedPlayersForAll(lobbyId);
			// sned an rpc for all joined players that a player has left
			foreach (var joinedPlayer in joinedPlayers!)
			{
				RpcId(
					joinedPlayer[nameof(IMattohaPlayer.Id)]!.GetValue<long>(),
					nameof(ClientRpc),
					nameof(MattohaClientRpcMethods.PlayerLeft),
					MattohaUtils.Serialize(MattohaUtils.ToHiddenPrivatePropsObject(player))
				);
			}
		}
		if (_sysetm?.AutoRefreshAvailableLobbies == true)
		{
			RefreshAvailableLobbiesForAll();
		}
		Middleware!.AfterPlayerLeaveLobby(this, player, lobby);
#endif
	}


	/// <summary>
	/// Server RPC sent by client to leave joined lobby.
	/// </summary>
	private void RpcLeaveLobby()
	{
#if MATTOHA_SERVER
		RemovePlayerFromJoinedLobby(Multiplayer.GetRemoteSenderId());
#endif
	}


	/// <summary>
	/// Server RPC sent by client to spawn a node for all players.
	/// </summary>
	/// <param name="jsonSpawnNodeInfo"></param>
	private void RpcSpawnNode(string jsonSpawnNodeInfo)
	{
#if MATTOHA_SERVER
		var ownerId = (long)Multiplayer.GetRemoteSenderId();
		var lobby = GetLobbyOfPlayer<JsonObject>(ownerId);
		if (lobby == null)
			return;
		var lobbyId = lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>();
		var ownerPlayer = GetPlayer<JsonObject>(ownerId);
		var info = MattohaUtils.Deserialize<MattohaSpawnNodeInfo>(jsonSpawnNodeInfo);
		var resp = Middleware!.BeforeSpawnNode(this, ownerPlayer!, lobby, info!);
		if (resp.Status)
		{
			SpawnedNodes[lobbyId].Add(info!);
			var players = GetPlayersInLobby<JsonObject>(lobbyId);
			foreach (var player in players!)
			{
				if (player[nameof(IMattohaPlayer.Id)]!.GetValue<long>() == ownerId)
					continue;
				RpcId(player[nameof(IMattohaPlayer.Id)]!.GetValue<long>(), nameof(ClientRpc), nameof(MattohaClientRpcMethods.SpawnNode), jsonSpawnNodeInfo);
			}
			Middleware!.AfterSpawnNode(this, ownerPlayer!, lobby, info!);
		}
		else
		{
			RpcId(ownerId, nameof(ClientRpcFail), nameof(MattohaFailType.SpawnNode), resp.Message);
		}
#endif
	}


	/// <summary>
	/// Server RPC sent by client to spawn nodes that already spawned from players.
	/// </summary>
	private void RpcSpawnAvailableNodes()
	{
#if MATTOHA_SERVER
		var playerId = (long)Multiplayer.GetRemoteSenderId();
		var lobby = GetLobbyOfPlayer<JsonObject>(playerId);
		if (lobby == null)
			return;
		var spawnedNodes = SpawnedNodes[lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>()];
		foreach (var node in spawnedNodes)
		{
			RpcId(playerId, nameof(ClientRpc), nameof(MattohaClientRpcMethods.SpawnNode), MattohaUtils.Serialize(node));
		}
#endif
	}


	/// <summary>
	/// Server RPC sent by client to despawn a node
	/// </summary>
	/// <param name="jsonSpawnNodeInfo">NodeInfo</param>
	private void RpcDespawnNode(string jsonSpawnNodeInfo)
	{
#if MATTOHA_SERVER
		var playerId = (long)Multiplayer.GetRemoteSenderId();
		var lobby = GetLobbyOfPlayer<JsonObject>(playerId);
		if (lobby == null)
			return;
		var info = MattohaUtils.Deserialize<MattohaSpawnNodeInfo>(jsonSpawnNodeInfo);
		var byPlayer = GetPlayer<JsonObject>(playerId);
		var resp = Middleware!.BeforeDespawnNode(this, byPlayer!, lobby, info!);
		if (resp.Status)
		{
			var nodeIndex = SpawnedNodes[lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>()]
				.FindIndex(node => node.OwnerId == playerId && node.ParentPath == info!.ParentPath && node.NodeName == info.NodeName);
			if (nodeIndex == -1)
				return;
			SpawnedNodes[lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>()].RemoveAt(nodeIndex);
			var joinedPlayers = GetPlayersInLobby<JsonObject>(lobby[nameof(IMattohaLobby.Id)]!.GetValue<long>());
			foreach (var player in joinedPlayers!)
			{
				RpcId(
					player[nameof(IMattohaPlayer.Id)]!.GetValue<long>(),
					nameof(ClientRpc), nameof(MattohaClientRpcMethods.DespawnNode),
					$"{info!.ParentPath}/{info!.NodeName}"
				);
			}
		}
		else
		{
			RpcId(playerId, nameof(ClientRpcFail), nameof(MattohaFailType.DespawnNode), resp.Message);
		}
#endif
	}


	/// <summary>
	/// Send a custom method name RPC to client to execute, this will emmit "UnhandledRpc" signal on client node.
	/// </summary>
	/// <param name="methodName">method name to handle.</param>
	/// <param name="paylaod">payload sent.</param>
	/// <param name="peer">peer id, 0 = all peers.</param>
	public void SendUnhandledClientRpc(string methodName, string paylaod, long peer = 0)
	{
#if MATTOHA_SERVER
		if (peer == 0)
		{
			Rpc(nameof(ClientRpc), methodName, paylaod);
		}
		else
		{
			RpcId(peer, nameof(ClientRpc), methodName, paylaod);
		}
#endif
	}


	public void ServerRpc(string method, string payload)
	{
#if MATTOHA_SERVER
		switch (method)
		{
			case nameof(MattohaServerRpcMethods.SetPlayerData):
				RpcSetPlayerData(payload);
				break;
			case nameof(MattohaServerRpcMethods.CreateLobby):
				RpcCreateLobby(payload);
				break;
			case nameof(MattohaServerRpcMethods.LoadAvailableLobbies):
				RpcLoadAvailableLobbies();
				break;
			case nameof(MattohaServerRpcMethods.JoinLobby):
				RpcJoinLobby(payload);
				break;
			case nameof(MattohaServerRpcMethods.RefreshJoinedLobby):
				RpcRefreshLobbyData();
				break;
			case nameof(MattohaServerRpcMethods.RefreshJoinedPlayers):
				RpcRefreshJoinedPlayers();
				break;
			case nameof(MattohaServerRpcMethods.SetLobbyData):
				RpcSetLobbyData(payload);
				break;
			case nameof(MattohaServerRpcMethods.SendMessage):
				RpcSendMessage(payload);
				break;
			case nameof(MattohaServerRpcMethods.StartGame):
				RpcStartGame();
				break;
			case nameof(MattohaServerRpcMethods.LeaveLobby):
				RpcLeaveLobby();
				break;
			case nameof(MattohaServerRpcMethods.SpawnNode):
				RpcSpawnNode(payload);
				break;
			case nameof(MattohaServerRpcMethods.SpawnAvailableNodes):
				RpcSpawnAvailableNodes();
				break;
			case nameof(MattohaServerRpcMethods.DespawnNode):
				RpcDespawnNode(payload);
				break;
			default:
				GD.Print("Unhandled Server RPC: " + method);
				EmitSignal(SignalName.UnhandledServerRpc, new MattohaSignal<string> { Value = method }, new MattohaSignal<string> { Value = payload });
				break;
		}
#endif
	}


	[Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ClientRpc(string method, string jsonPayload)
	{
#if MATTOHA_CLIENT
		_sysetm?.Client?.ClientRpc(method, jsonPayload);
#endif
	}


	[Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ClientRpcFail(string mattohaFailType, string failCause)
	{
#if MATTOHA_CLIENT
		_sysetm?.Client?.ClientRpcFail(mattohaFailType, failCause);
#endif
	}
}
