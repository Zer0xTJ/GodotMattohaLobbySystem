using MattohaLobbySystem.Core.Interfaces;
using System.Collections.Generic;

namespace MattohaLobbySystem.Demo.Models;

public partial class PlayerModel : IMattohaPlayer
{
	public long Id { get; set; }
	public long JoinedLobbyId { get; set; }
	public int TeamId { get; set; }
	public string Username { get; set; } = "New Player";
	public List<string> ChatProps { get; set; } = new() { nameof(Id), nameof(Username) };
	public List<string> PrivateProps { get; set; } = new();


	public override string ToString()
	{
		return $"Player({Id}, {Username}, Team={TeamId}, Lobby={JoinedLobbyId})";
	}
}
