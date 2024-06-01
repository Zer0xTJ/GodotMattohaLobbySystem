using Godot;
namespace Mattoha.Nodes;

public partial class MattohaSpawner : Node
{
    [Export] public bool AutoSpawn { get; set; } = true;
    [Export] public bool AutoDespawn { get; set; } = true;

    public override void _Ready()
    {
        if (AutoSpawn && IsMultiplayerAuthority())
        {
            GD.Print("AUOOOOO");
            MattohaSystem.Instance.Client.SpawnNode(GetParent());
        }
        base._Ready();
    }

    public override void _ExitTree()
    {
        if (AutoDespawn && (Multiplayer.GetUniqueId() == 1 || IsMultiplayerAuthority()))
        {
            MattohaSystem.Instance.Client.DespawnNode(GetParent());
        }
        base._ExitTree();
    }
}