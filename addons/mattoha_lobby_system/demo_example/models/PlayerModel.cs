using Godot;
using Godot.Collections;

namespace MattohaLobbySystem.Demo.Models;

public partial class PlayerModel
{
	public long Id { get; set; }
	public long JoinedLobbyId { get; set; }
	public int TeamId { get; set; }
	public string Username { get; set; } = "New Player";
	public Array<string> ChatProps { get; set; } = new() { nameof(Id), nameof(Username) };
	public Array<string> PrivateProps { get; set; } = new();

	public Dictionary<string, Variant> ToDict()
	{
		return new Dictionary<string, Variant>()
		{
			{ nameof(Id), Id },
			{ nameof(JoinedLobbyId), JoinedLobbyId },
			{ nameof(TeamId), TeamId },
			{ nameof(Username), Username },
			{ nameof(ChatProps), ChatProps },
			{ nameof(PrivateProps), PrivateProps },
		};
	}

	public override string ToString()
	{
		return $"Player({Id}, {Username}, Team={TeamId}, Lobby={JoinedLobbyId})";
	}
}
