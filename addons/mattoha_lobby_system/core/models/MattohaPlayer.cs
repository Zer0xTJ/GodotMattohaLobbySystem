using Godot;
using Godot.Collections;

namespace MattohaLobbySystem.Core.Models;
public class MattohaPlayer
{
    public long Id { get; set; }
    public string Username { get; set; } = "New Player";
    public int JoinedLobbyId { get; set; }
    public int TeamId { get; set; }
    public Array<string> PrivateProps { get; set; } = new();
    public Array<string> ChatProps { get; set; } = new Array<string> { nameof(Id), nameof(Username) };

    public virtual Dictionary<string, Variant> ToDict()
    {
        return new Dictionary<string, Variant>()
        {
            { nameof(Id), Id },
            { nameof(Username), Username },
            { nameof(JoinedLobbyId), JoinedLobbyId },
            { nameof(TeamId), TeamId },
            { nameof(PrivateProps), PrivateProps },
            { nameof(ChatProps), ChatProps },
        };
    }
}