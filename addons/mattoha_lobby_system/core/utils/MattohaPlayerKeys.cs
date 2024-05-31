using MattohaLobbySystem.Core.Models;
using System;
using System.Collections.Generic;

namespace MattohaLobbySystem.Core.Utils;

static class MattohaPlayerKeys
{
    public static string Id { get; set; } = nameof(MattohaPlayer.Id);
    public static string Username { get; set; } = nameof(MattohaPlayer.Username);
    public static string JoinedLobbyId { get; set; } = nameof(MattohaPlayer.JoinedLobbyId);
    public static string TeamId { get; set; } = nameof(MattohaPlayer.TeamId);
    public static string PrivateProps { get; set; } = nameof(MattohaPlayer.PrivateProps);
    public static string ChatProps { get; set; } = nameof(MattohaPlayer.ChatProps);

    public static List<string> FreezedProperties = new() { Id, JoinedLobbyId, TeamId };

}
