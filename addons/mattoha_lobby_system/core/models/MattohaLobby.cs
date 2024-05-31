using Godot;
using Godot.Collections;

namespace MattohaLobbySystem.Core.Models;
public partial class MattohaLobby
{
    public int Id { get; set; }
    public long OwnerId { get; set; }
    public string Name { get; set; } = "New Lobby";
    public int MaxPlayers { get; set; }
    public int PlayersCount { get; set; }
    public bool IsGameStarted { get; set; }
    public Array<string> PrivateProps { get; set; } = new();

    public virtual Dictionary<string, Variant> ToDict()
    {
        return new Dictionary<string, Variant>(){
            { nameof(Id), Id },
            { nameof(OwnerId), OwnerId },
            { nameof(Name), Name },
            { nameof(MaxPlayers), MaxPlayers },
            { nameof(PlayersCount), PlayersCount },
            { nameof(IsGameStarted), IsGameStarted },
            { nameof(PrivateProps), PrivateProps },
        };
    }
}