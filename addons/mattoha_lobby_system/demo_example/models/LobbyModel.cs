using MattohaLobbySystem.Core.Models;

namespace MattohaLobbySystem.Demo.Models;

public partial class LobbyModel : MattohaLobby
{
	public override string ToString()
	{
		return $"Lobby({Id}, {Name}, owner={OwnerId}, Players={PlayersCount}/{MaxPlayers}, Started={IsGameStarted})";
	}
}
