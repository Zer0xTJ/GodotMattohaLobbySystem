using System.Collections.Generic;

namespace MattohaLobbySystem.Core.Interfaces;

public interface IMattohaChatProps
{
	/// <summary>
	/// List of property names that will be sent with message to other players.
	/// </summary>
	public List<string> ChatProps { get; set; }
}
