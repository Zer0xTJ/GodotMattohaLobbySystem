using Godot;
using Godot.Collections;
using Mattoha.Core.Demo;
using System;

namespace Mattoha.Nodes;
public partial class MattohaClient : Node
{
	[Signal] public delegate void ConnectedToServerEventHandler();
	[Signal] public delegate void PlayerRegisteredEventHandler(Dictionary<string, Variant> playerData);

	[Signal] public delegate void SetPlayerDataSucceedEventHandler(Dictionary<string, Variant> playerData);
	[Signal] public delegate void SetPlayerDataFailedEventHandler(string cause);


	[Signal] public delegate void CreateLobbySucceedEventHandler(Dictionary<string, Variant> lobbyData);
	[Signal] public delegate void CreateLobbyFailedEventHandler(string cause);
	
	[Signal] public delegate void LoadAvailableLobbiesSucceedEventHandler(Array<Dictionary<string, Variant>> lobbies);

	public Dictionary<string, Variant> CurrentPlayer { get; set; } = new();
	public Dictionary<string, Variant> CurrentLobby { get; set; } = new();
	public Array<long> CurrentLobbyPlayers { get; set; } = new() { 1 };

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


	public void CreateLobby(Dictionary<string, Variant> lobbyData)
	{
#if MATTOHA_CLIENT
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
		_system.SendReliableServerRpc(nameof(ServerRpc.LoadAvailableLobbies), new());
#endif
	}


	private void RpcLoadAvailableLobbies(Dictionary<string, Variant> lobbiesPayload)
	{
#if MATTOHA_CLIENT
		Array<Dictionary<string, Variant>> lobbies = lobbiesPayload["Lobbies"].AsGodotArray<Dictionary<string, Variant>>();
		EmitSignal(SignalName.LoadAvailableLobbiesSucceed, lobbies);
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
		}
#endif
	}

}
