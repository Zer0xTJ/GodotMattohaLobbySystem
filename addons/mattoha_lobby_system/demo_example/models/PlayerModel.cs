using MattohaLobbySystem.Core.Models;

namespace MattohaLobbySystem.Demo.Models;

public partial class PlayerModel : MattohaPlayer
{
	public override string ToString()
	{
		return $"Player({Id}, {Username}, Team={TeamId}, Lobby={JoinedLobbyId})";
	}
}
