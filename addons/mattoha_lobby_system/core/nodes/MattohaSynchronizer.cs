using Godot;

namespace Mattoha.Nodes;
public partial class MattohaSynchronizer : MultiplayerSynchronizer
{
    [Export] public bool ReplicateForTeamOnly { get; set; } = false;

    public override void _Ready()
    {
        ApplyMattohaReplicationFilter();
        base._Ready();
    }
    public virtual void ApplyMattohaReplicationFilter()
    {
        var mattohaFilter = new Callable(this, nameof(MattohaFilter));
        AddVisibilityFilter(mattohaFilter);
    }

    private bool MattohaFilter(long peerId)
    {
        if (ReplicateForTeamOnly)
        {
            return MattohaSystem.Instance.Client.ShouldReplicate && MattohaSystem.Instance.Client.IsPlayerInMyTeam(peerId);
        }
        else
        {
            return MattohaSystem.Instance.Client.ShouldReplicate && MattohaSystem.Instance.Client.IsPlayerInLobby(peerId);
        }
    }
}