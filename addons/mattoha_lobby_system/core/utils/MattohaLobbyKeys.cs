
using MattohaLobbySystem.Core.Models;
using System.Collections.Generic;

namespace MattohaLobbySystem.Core.Utils;

public static class MattohaLobbyKeys
{
	public static string Id { get; set; } = nameof(MattohaLobby.Id);
	public static string OwnerId { get; set; } = nameof(MattohaLobby.OwnerId);
	public static string Name { get; set; } = nameof(MattohaLobby.Name);
	public static string MaxPlayers { get; set; } = nameof(MattohaLobby.MaxPlayers);
	public static string PlayersCount { get; set; } = nameof(MattohaLobby.PlayersCount);
	public static string IsGameStarted { get; set; } = nameof(MattohaLobby.IsGameStarted);
	public static string PrivateProps { get; set; } = nameof(MattohaLobby.PrivateProps);

	public static List<string> FreezedProperties = new() { Id, OwnerId, MaxPlayers, PlayersCount, IsGameStarted };

}