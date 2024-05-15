using MattohaLobbySystem.Core.Interfaces;
using System.Collections.Generic;

namespace MattohaLobbySystem.Demo.Models;

public partial class LobbyModel : IMattohaLobby
{
	public long Id { get; set; }
	public long OwnerId { get; set; }
	public string Name { get; set; } = "New Lobby";
	public int MaxPlayers { get; set; }
	public int PlayersCount { get; set; }
	public bool IsGameStarted { get; set; }
	public List<string> PrivateProps { get; set; } = [];

	public override string ToString()
	{
		return $"Lobby({Id}, {Name}, owner={OwnerId}, Players={PlayersCount}/{MaxPlayers}, Started={IsGameStarted})";
	}
}
