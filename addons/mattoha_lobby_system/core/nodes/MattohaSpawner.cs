using Godot;
namespace Mattoha.Nodes;

public partial class MattohaSpawner : Node
{
    [Export] public bool AutoSpawn { get; set; } = true;
    [Export] public bool AutoDespawn { get; set; } = true;
    [Export] public bool SpawnForTeamOnly { get; set; } = false;
    [Export] public bool HandleByServer { get; set; } = false;

    public override void _Ready()
    {
        if (AutoSpawn && HandleByServer && Multiplayer.IsServer())
        {
#if MATTOHA_SERVER
            MattohaSystem.Instance.Server.SpawnNode(
                MattohaSystem.Instance.GenerateNodePayloadData(GetParent()),
                MattohaSystem.ExtractLobbyId(GetParent().GetPath())
            );
#endif
        }
        else if (AutoSpawn && !HandleByServer && !Multiplayer.IsServer())
        {
#if MATTOHA_CLIENT
            MattohaSystem.Instance.Client.SpawnNode(GetParent(), SpawnForTeamOnly);
#endif
        }
        base._Ready();
    }

    public override void _ExitTree()
    {
        if (AutoDespawn)
        {
            if (HandleByServer && Multiplayer.IsServer())
            {
#if MATTOHA_SERVER
                MattohaSystem.Instance.Server.DespawnNode(MattohaSystem.Instance.GenerateNodePayloadData(GetParent()));
#endif
            }
            else if (!HandleByServer && !Multiplayer.IsServer())
            {
#if MATTOHA_CLIENT
                MattohaSystem.Instance.Client.DespawnNode(GetParent());
#endif
            }
        }
        base._ExitTree();
    }
}