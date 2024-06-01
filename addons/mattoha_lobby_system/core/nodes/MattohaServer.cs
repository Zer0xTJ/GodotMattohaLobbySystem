using Godot;
using Godot.Collections;
using Mattoha.Core.Demo;
using Mattoha.Core;
using Mattoha.Core.Utils;
using System;
using System.Linq;

namespace Mattoha.Nodes;
public partial class MattohaServer : Node
{
	[Export] public Dictionary<long, Dictionary<string, Variant>> Players { get; set; } = new();
	[Export] public Dictionary<int, Dictionary<string, Variant>> Lobbies { get; set; } = new();
	public Node GameHolder => GetNode("/root/GameHolder");

	private MattohaSystem _system;

	public override void _Ready()
	{
		_system = GetParent<MattohaSystem>();
		_system.ServerRpcRecieved += OnServerRpcRecieved;
		Multiplayer.PeerConnected += OnPeerConnected;
		base._Ready();
	}


	private void OnPeerConnected(long id)
	{
#if MATTOHA_SERVER
		if (Multiplayer.IsServer())
		{
			RegisterPlayer(id);
		}
#endif
	}




	public Dictionary<string, Variant> GetPlayerLobby(long playerId)
	{
		if (!Players.TryGetValue(playerId, out var player))
			return null;

		if (!Lobbies.TryGetValue(player[MattohaPlayerKeys.JoinedLobbyId].AsInt32(), out var lobby))
			return null;

		return lobby;
	}

	public Array<Dictionary<string, Variant>> GetLobbyPlayers(int lobbyId)
	{
		var players = new Array<Dictionary<string, Variant>>();
		foreach (var pl in Players.Values)
		{
			players.Add(pl);
		}
		return players;
	}


	public Array<Dictionary<string, Variant>> GetLobbyPlayersSecured(int lobbyId)
	{
		var players = new Array<Dictionary<string, Variant>>();
		foreach (var pl in Players.Values)
		{
			players.Add(MattohaUtils.ToSecuredDict(pl));
		}
		return players;
	}


	public void SendRpcForPlayersInLobby(int lobbyId, string methodName, Dictionary<string, Variant> payload, bool secureDict = false, long superPeer = 0)
	{
		var players = GetLobbyPlayers(lobbyId);
		foreach (var player in players)
		{
			var playerId = player[MattohaPlayerKeys.Id].AsInt64();
			if (superPeer != 0 && playerId == superPeer)
			{
				_system.SendReliableClientRpc(playerId, methodName, payload);
			}
			else if (secureDict)
			{
				_system.SendReliableClientRpc(playerId, methodName, MattohaUtils.ToSecuredDict(payload));
			}
			else
			{
				_system.SendReliableClientRpc(playerId, methodName, payload);
			}
		}
	}


	private long CalculateLobbyXPosition()
	{
		// because adding the lobby node is done (after) adding lobby details to lobbies dictionary,
		// we wil substract the last added lobby (current lobby)
		// this will lead to start positioning lobbies from 0 to LobbySize * n
		// if we removed "-1" it will start positioning from lobbySize to LobbySize * n
		return (Lobbies.Count - 1) * _system.LobbySize;
	}


	private void RegisterPlayer(long id)
	{
#if MATTOHA_SERVER
		if (!Players.ContainsKey(id))
		{
			var player = new Dictionary<string, Variant>
				{
					{ MattohaPlayerKeys.Id, id },
					{ MattohaPlayerKeys.Username, $"Player{id}" },
					{ MattohaPlayerKeys.JoinedLobbyId, 0 },
					{ MattohaPlayerKeys.TeamId, 0 },
					{ MattohaPlayerKeys.PrivateProps, new Array<string>() },
					{ MattohaPlayerKeys.ChatProps, new Array<string>(){ MattohaPlayerKeys.Id, MattohaPlayerKeys.Username } },
				};
			Players.Add(id, player);
			_system.SendReliableClientRpc(id, nameof(ClientRpc.RegisterPlayer), player);
		}
#endif
	}


	private void RpcSetPlayerData(Dictionary<string, Variant> payload, long sender)
	{
#if MATTOHA_SERVER
		if (!Players.TryGetValue(sender, out var player))
			return;
		foreach (var pair in payload)
		{
			if (MattohaPlayerKeys.FreezedProperties.Contains(pair.Key))
				continue;
			player[pair.Key] = pair.Value;
		}
		_system.SendReliableClientRpc(sender, nameof(ClientRpc.SetPlayerData), player);
		// tood: send that a player updated his data to all joined players in lobby
#endif
	}


	private void RpcCreateLobby(Dictionary<string, Variant> lobbyData, long sender)
	{
#if MATTOHA_SERVER
		if (!Players.TryGetValue(sender, out var player))
			return;
		var lobbyId = Lobbies.Count + 1;
		lobbyData[MattohaLobbyKeys.Id] = lobbyId;
		lobbyData[MattohaLobbyKeys.OwnerId] = sender;
		lobbyData[MattohaLobbyKeys.PlayersCount] = 1;
		lobbyData[MattohaLobbyKeys.IsGameStarted] = false;
		if (lobbyData[MattohaLobbyKeys.PrivateProps].Obj == null)
		{
			lobbyData[MattohaLobbyKeys.PrivateProps] = new Array<string>();
		}
		var maxPlayers = _system.MaxPlayersPerLobby;
		if (lobbyData.ContainsKey(MattohaLobbyKeys.MaxPlayers))
		{
			maxPlayers = Math.Min(lobbyData[MattohaLobbyKeys.MaxPlayers].AsInt32(), _system.MaxPlayers);
		}
		lobbyData[MattohaLobbyKeys.MaxPlayers] = maxPlayers;
		player[MattohaPlayerKeys.JoinedLobbyId] = lobbyId;

		Lobbies.Add(lobbyId, lobbyData);

		// add lobby node to game holder
		var lobbyNode = GD.Load<PackedScene>(lobbyData[MattohaLobbyKeys.LobbySceneFile].AsString()).Instantiate();
		if (lobbyNode is Node2D)
			(lobbyNode as Node2D).Position = new Vector2(CalculateLobbyXPosition(), 0);
		if (lobbyNode is Node3D)
			(lobbyNode as Node3D).Position = new Vector3(CalculateLobbyXPosition(), 0, 0);
		lobbyNode.Name = $"Lobby{lobbyId}";
		GameHolder.AddChild(lobbyNode);

		_system.SendReliableClientRpc(sender, nameof(ClientRpc.CreateLobby), lobbyData);
		_system.SendReliableClientRpc(sender, nameof(ClientRpc.SetPlayerData), player);
		RefreshAvailableLobbiesForAll();
#endif
	}


	public void RefreshAvailableLobbiesForAll()
	{
#if MATTOHA_SERVER
		if (_system.AutoLoadAvailableLobbies)
		{
			foreach (var playerId in Players.Keys)
			{
				RpcLoadAvailableLobbies(playerId);
			}

		}
#endif
	}

	private void RpcLoadAvailableLobbies(long sender)
	{
#if MATTOHA_SERVER
		Array<Dictionary<string, Variant>> lobbies = new();
		foreach (var lobbyData in Lobbies.Values)
		{
			lobbies.Add(MattohaUtils.ToSecuredDict(lobbyData));
		}
		Dictionary<string, Variant> payload = new()
		{
			{ "Lobbies", lobbies }
		};
		_system.SendReliableClientRpc(sender, nameof(ClientRpc.LoadAvailableLobbies), payload);
#endif
	}


	private void RpcJoinLobby(Dictionary<string, Variant> payload, long sender)
	{
#if MATTOHA_SERVER
		if (!Players.TryGetValue(sender, out var player))
			return;
		// todo: remove player from joined lobby

		var lobbyId = payload[MattohaLobbyKeys.Id].AsInt32();
		if (!Lobbies.TryGetValue(lobbyId, out var lobby))
			return;

		if (lobby[MattohaLobbyKeys.MaxPlayers].AsInt32() <= lobby[MattohaLobbyKeys.PlayersCount].AsInt32())
			return;

		player[MattohaPlayerKeys.JoinedLobbyId] = lobbyId;
		lobby[MattohaLobbyKeys.PlayersCount] = lobby[MattohaLobbyKeys.PlayersCount].AsInt32() + 1;

		_system.SendReliableClientRpc(sender, nameof(ClientRpc.SetPlayerData), player);
		_system.SendReliableClientRpc(sender, nameof(ClientRpc.JoinLobby), MattohaUtils.ToSecuredDict(lobby));

		// todo: refresh joined players for all joined players in lobby
		RefreshAvailableLobbiesForAll();

#endif
	}


	private void RpcStartGame(long sender)
	{
#if MATTOHA_SERVER
		var lobby = GetPlayerLobby(sender);
		if (lobby == null)
			return;

		if (lobby[MattohaLobbyKeys.OwnerId].AsInt64() != sender)
			return;

		lobby[MattohaLobbyKeys.IsGameStarted] = true;
		SendRpcForPlayersInLobby(lobby[MattohaLobbyKeys.Id].AsInt32(), nameof(ClientRpc.StartGame), null);

#endif
	}


	private void RpcLoadLobbyPlayers(long sender)
	{
#if MATTOHA_SERVER
		if (!Players.TryGetValue(sender, out var player))
			return;

		var joinedLobbyId = player[MattohaPlayerKeys.JoinedLobbyId].AsInt32();
		var players = GetLobbyPlayersSecured(joinedLobbyId);
		var payload = new Dictionary<string, Variant>()
		{
			{ "Players", players }
		};
		SendRpcForPlayersInLobby(joinedLobbyId, nameof(ClientRpc.LoadLobbyPlayers), payload);
#endif
	}

	private void OnServerRpcRecieved(string methodName, Dictionary<string, Variant> payload, long sender)
	{
#if MATTOHA_SERVER
		switch (methodName)
		{
			case nameof(ServerRpc.SetPlayerData):
				RpcSetPlayerData(payload, sender);
				break;
			case nameof(ServerRpc.CreateLobby):
				RpcCreateLobby(payload, sender);
				break;
			case nameof(ServerRpc.LoadAvailableLobbies):
				RpcLoadAvailableLobbies(sender);
				break;
			case nameof(ServerRpc.JoinLobby):
				RpcJoinLobby(payload, sender);
				break;
			case nameof(ServerRpc.StartGame):
				RpcStartGame(sender);
				break;
			case nameof(ServerRpc.LoadLobbyPlayers):
				RpcLoadLobbyPlayers(sender);
				break;
			case nameof(ServerRpc.SpawnNode):
				RpcSpawnNode(payload, sender);
				break;
		}
#endif
	}

	private void RpcSpawnNode(Dictionary<string, Variant> payload, long sender)
	{
#if MATTOHA_SERVER
		var lobbyId = MattohaSystem.ExtractLobbyId(payload[MattohaSpawnKeys.ParentPath].ToString());
		if (!Lobbies.TryGetValue(lobbyId, out var lobby))
			return;
		_system.SpawnNodeFromPayload(payload);
		SendRpcForPlayersInLobby(lobbyId, nameof(ClientRpc.SpawnNode), payload);
#endif
	}
}
