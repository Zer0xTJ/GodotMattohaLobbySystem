using Godot;
using Godot.Collections;
using Mattoha.Core.Demo;
using MattohaLobbySystem.Core.Utils;
using System;

namespace Mattoha.Nodes;
public partial class MattohaServer : Node
{
	[Export] public Dictionary<long, Dictionary<string, Variant>> Players { get; set; } = new();
	[Export] public Dictionary<int, Dictionary<string, Variant>> Lobbies { get; set; } = new();
	[Export] public Dictionary<long, Array<Dictionary<string, Variant>>> SpawnedNodes { get; set; } = new();
	[Export] public Dictionary<long, Array<Dictionary<string, Variant>>> RemovedSceneNodes { get; set; } = new();

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
					{ MattohaPlayerKeys.ChatProps, new Array<string>(){ nameof(MattohaPlayerKeys.Id), nameof(MattohaPlayerKeys.Username) } },
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
		lobbyData[MattohaLobbyKeys.PrivateProps] = new Array<string>();
		var maxPlayers = _system.MaxPlayersPerLobby;
		if (lobbyData.ContainsKey(MattohaLobbyKeys.MaxPlayers))
		{
			maxPlayers = Math.Min(lobbyData[MattohaLobbyKeys.MaxPlayers].AsInt32(), _system.MaxPlayers);
		}
		lobbyData[MattohaLobbyKeys.MaxPlayers] = maxPlayers;
		
		player[MattohaPlayerKeys.JoinedLobbyId] = lobbyId;
		_system.SendReliableClientRpc(sender, nameof(ClientRpc.CreateLobby), lobbyData);
		_system.SendReliableClientRpc(sender, nameof(ClientRpc.SetPlayerData), player);
		// todo: refresh lobbies list for all
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
		}
#endif
	}
}
