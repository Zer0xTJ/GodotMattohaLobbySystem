
using Godot;
using Godot.Collections;

namespace MattohaLobbySystem.Demo.Models;

public partial class LobbyModel
{
	public long Id { get; set; }
	public long OwnerId { get; set; }
	public string Name { get; set; } = "New Lobby";
	public int MaxPlayers { get; set; }
	public int PlayersCount { get; set; }
	public bool IsGameStarted { get; set; }
	public Array<string> PrivateProps { get; set; } = new();


	public Dictionary<string, Variant> ToDict()
	{
		return new Dictionary<string, Variant>()
		{
			{ nameof(Id), Id },
			{ nameof(OwnerId), OwnerId },
			{ nameof(Name), Name },
			{ nameof(MaxPlayers), MaxPlayers },
			{ nameof(PlayersCount), PlayersCount },
			{ nameof(IsGameStarted), IsGameStarted },
		};
	}

	public override string ToString()
	{
		return $"Lobby({Id}, {Name}, owner={OwnerId}, Players={PlayersCount}/{MaxPlayers}, Started={IsGameStarted})";
	}
}
