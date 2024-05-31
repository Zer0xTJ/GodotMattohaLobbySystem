
using Godot;

namespace Mattoha.Nodes;
public partial class MattohaSystem : Node
{
	[Export] public string Address { get; set; } = "127.0.0.1";
	[Export] public int Port { get; set; } = 7001;
	[Export] public int MaxPlayers { get; set; } = 4000;
	[Export] public int MaxPlayersPerLobby { get; set; } = 10;
	public int MaxLobbies => MaxPlayers / MaxPlayersPerLobby;
	[Export] public int LobbySize { get; set; } = 2500;
	[Export] public MattohaServer Server { get; set; }
	[Export] public MattohaClient Client { get; set; }


	public void StartServer()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateServer(Port, MaxPlayers);
		Multiplayer.MultiplayerPeer = peer;
	}


	public void StartClient()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateClient(Address, Port);
		Multiplayer.MultiplayerPeer = peer;
	}
}