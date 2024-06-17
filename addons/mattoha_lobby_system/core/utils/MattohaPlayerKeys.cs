using System.Collections.Generic;

namespace Mattoha.Core.Utils;

static class MattohaPlayerKeys
{
    public static string Id { get; private set; } = "Id";
    public static string Username { get; private set; } = "Username";
    public static string JoinedLobbyId { get; private set; } = "JoinedLobbyId";
    public static string TeamId { get; private set; } = "TeamId";
    public static string IsInGame { get; private set; } = "IsInGame";
    public static string PrivateProps { get; private set; } = "PrivateProps";
    public static string ChatProps { get; private set; } = "ChatProps";

    public static List<string> FreezedProperties { get; private set; } = new() { Id, JoinedLobbyId, IsInGame };

}
