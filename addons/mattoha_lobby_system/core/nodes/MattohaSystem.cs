using Godot;
using Godot.Collections;
using MattohaLobbySystem.Core;

namespace Mattoha.Nodes;
public partial class MattohaSystem : Node
{

	[Signal] public delegate void ServerRpcRecievedEventHandler(string methodName, Dictionary<string, Variant> payload, long sender);
	[Signal] public delegate void ClientRpcRecievedEventHandler(string methodName, Dictionary<string, Variant> payload, long sender);

	[ExportGroup("Server Configuration"), Export] public string Address { get; set; } = "127.0.0.1";
	[ExportGroup("Server Configuration"), Export] public int Port { get; set; } = 7001;
	[ExportGroup("Server Configuration"), Export] public int MaxChannels { get; set; } = 5;
	[ExportGroup("Server Configuration"), Export] public int MaxPlayers { get; set; } = 4000;
	[ExportGroup("Server Configuration"), Export] public int MaxPlayersPerLobby { get; set; } = 10;
	public int MaxLobbies => MaxPlayers / MaxPlayersPerLobby;

	[ExportGroup("Server Configuration"), Export] public bool AutoLoadAvailableLobbies { get; set; } = true;
	[ExportGroup("Server Configuration"), Export] public bool DespawnPlayerNodesOnLeave { get; set; } = true;

	[ExportGroup("System Nodes"), Export] public MattohaServer Server { get; set; }
	[ExportGroup("System Nodes"), Export] public MattohaClient Client { get; set; }
	
	[ExportGroup("Lobby Configuration"), Export] public long LobbySize { get; set; } = 2500;

	
	public static MattohaSystem Instance { get; set; }


	public override void _Ready()
	{
		Instance = this;
		base._Ready();
	}

	public void StartServer()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateServer(Port, MaxPlayers, MaxChannels);
		Multiplayer.MultiplayerPeer = peer;
	}


	public void StartClient()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateClient(Address, Port);
		Multiplayer.MultiplayerPeer = peer;
	}

	
	public void SendReliableServerRpc(string methodName, Dictionary<string, Variant> payload)
	{
		RpcId(1, nameof(ServerReliableRpc), methodName, payload);
	}

	
	public void SendReliableClientRpc(long peer, string methodName, Dictionary<string, Variant> payload)
	{
		RpcId(peer, nameof(ClientReliableRpc), methodName, payload);
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, TransferChannel = 2)]
	private void ServerReliableRpc(string methodName, Dictionary<string, Variant> payload)
	{
#if MATTOHA_SERVER
		EmitSignal(SignalName.ServerRpcRecieved, methodName, payload, Multiplayer.GetRemoteSenderId());
#endif
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, TransferChannel = 2)]
	private void ClientReliableRpc(string methodName, Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.ClientRpcRecieved, methodName, payload, Multiplayer.GetRemoteSenderId());
#endif
	}
}