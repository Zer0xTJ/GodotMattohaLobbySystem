using System.Collections.Generic;

namespace Mattoha.Core.Utils;

public static class MattohaLobbyKeys
{
	public static string Id { get; set; } = "Id";
	public static string OwnerId { get; set; } = "OwnerId";
	public static string Name { get; set; } = "Name";
	public static string MaxPlayers { get; set; } = "MaxPlayers";
	public static string PlayersCount { get; set; } = "PlayersCount";
	public static string IsGameStarted { get; set; } = "IsGameStarted";
	public static string PrivateProps { get; set; } = "PrivateProps";
	public static string LobbySceneFile { get; set; } = "LobbySceneFile";

	public static List<string> FreezedProperties = new() { Id, OwnerId, PlayersCount, IsGameStarted };

}