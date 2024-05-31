using Godot;
using Godot.Collections;

namespace Mattoha.Nodes;
public partial class MattohaSystem : Node
{

	[Signal] public delegate void ServerRpcRecievedEventHandler(string methodName, Dictionary<string, Variant> payload, long sender);
	[Signal] public delegate void ClientRpcRecievedEventHandler(string methodName, Dictionary<string, Variant> payload, long sender);

	[Export] public string Address { get; set; } = "127.0.0.1";
	[Export] public int Port { get; set; } = 7001;
	[Export] public int MaxChannels { get; set; } = 5;
	[Export] public int MaxPlayers { get; set; } = 4000;
	[Export] public int MaxPlayersPerLobby { get; set; } = 10;
	public int MaxLobbies => MaxPlayers / MaxPlayersPerLobby;
	[Export] public int LobbySize { get; set; } = 2500;
	[Export] public MattohaServer Server { get; set; }
	[Export] public MattohaClient Client { get; set; }
	
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