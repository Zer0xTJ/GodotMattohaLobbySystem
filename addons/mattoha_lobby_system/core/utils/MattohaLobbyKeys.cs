using System.Collections.Generic;

namespace Mattoha.Core.Utils;

public static class MattohaLobbyKeys
{
	public static string Id { get; private set; } = "Id";
	public static string OwnerId { get; private set; } = "OwnerId";
	public static string Name { get; private set; } = "Name";
	public static string MaxPlayers { get; private set; } = "MaxPlayers";
	public static string PlayersCount { get; private set; } = "PlayersCount";
	public static string IsGameStarted { get; private set; } = "IsGameStarted";
	public static string PrivateProps { get; private set; } = "PrivateProps";
	public static string LobbySceneFile { get; private set; } = "LobbySceneFile";

	public static List<string> FreezedProperties { get; private set; } = new() { Id, OwnerId, PlayersCount, IsGameStarted };

}