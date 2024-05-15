using System.Collections.Generic;

namespace MattohaLobbySystem.Core.Interfaces;

public interface IMattohaPrivateProps
{
	/// <summary>
	/// List of property names that will be hidden when player data is sent to other players.
	/// </summary>
	public List<string> PrivateProps { get; set; }
}
