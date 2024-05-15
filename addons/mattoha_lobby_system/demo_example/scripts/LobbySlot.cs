using Godot;
using MattohaLobbySystem.Demo.Models;

namespace MattohaLobbySystem.Demo;
public partial class LobbySlot : HBoxContainer
{
	public LobbyModel? Lobby { get; set; }

	[Export] Label? LobbyNameLabel { get; set; }
	[Export] Label? PlayersCOuntLabel { get; set; }

	public override void _EnterTree()
	{
		LobbyNameLabel!.Text = Lobby?.Name ?? "Lobby Name";
		PlayersCOuntLabel!.Text = $"{Lobby?.PlayersCount}/{Lobby?.MaxPlayers} players";
		base._EnterTree();
	}

	public void OnJoinButtonPressed()
	{
		MyLobbyManager.System?.Client?.JoinLobby(Lobby!.Id);
	}
}
