using System.Collections.Generic;

namespace MattohaLobbySystem.Core.Utils;

static class MattohaPlayerKeys
{
    public static string Id { get; set; } = "Id";
    public static string Username { get; set; } = "Username";
    public static string JoinedLobbyId { get; set; } = "JoinedLobbyId";
    public static string TeamId { get; set; } = "TeamId";
    public static string PrivateProps { get; set; } = "PrivateProps";
    public static string ChatProps { get; set; } = "ChatProps";

    public static List<string> FreezedProperties = new() { Id, JoinedLobbyId, TeamId };

}
