using Godot;
namespace Mattoha.Nodes;

public partial class MattohaSpawner : Node
{
    [Export] public bool AutoSpawn { get; set; } = true;
    [Export] public bool AutoDespawn { get; set; } = true;

    // this will cause the despawning happens from server, not client, bcs despawning nodes can be from owner only
    // but scene nodes has no owner
    [Export] public bool IsSceneNode { get; set; } = true;

    public override void _Ready()
    {
        if (AutoSpawn && IsMultiplayerAuthority() && !IsSceneNode)
        {
            MattohaSystem.Instance.Client.SpawnNode(GetParent());
        }
        base._Ready();
    }

    public override void _ExitTree()
    {
        if (AutoDespawn && (Multiplayer.GetUniqueId() == 1 || IsMultiplayerAuthority()))
        {
            if (Multiplayer.GetUniqueId() == 1 && IsSceneNode)
            {
#if MATTOHA_SERVER
                MattohaSystem.Instance.Server.DespawnNode(MattohaSystem.Instance.GenerateNodePayloadData(GetParent()));
#endif
            }
            else
            {
                MattohaSystem.Instance.Client.DespawnNode(GetParent());
            }
        }
        base._ExitTree();
    }
}