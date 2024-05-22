using System.Collections.Generic;

namespace MattohaLobbySystem.Core.Interfaces;

public interface IMattohaPlayer : IMattohaChatProps, IMattohaPrivateProps
{
	public long Id { get; set; }

	/// <summary>
	/// Currently joined lobby Id, 0 if not joined.
	/// </summary>
	public long JoinedLobbyId { get; set; }

	/// <summary>
	/// The team id of the player in lobby, not globally.
	/// </summary>
	public int TeamId { get; set; }

	public string Username { get; set; }
}
