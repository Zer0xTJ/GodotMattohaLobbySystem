using Godot;
using Godot.Collections;
namespace Mattoha.Nodes;

public partial class MattohaSpawner : Node
{
	/// <summary>
	/// when true, will spawn node for other players on _ready().
	/// </summary>
	[Export] public bool AutoSpawn { get; set; } = true;
    
    /// <summary>
    /// when true, will despawn node for others when queue_free().
    /// </summary>
    [Export] public bool AutoDespawn { get; set; } = true;
    
    /// <summary>
    /// when true, spawning the node will be only for team members.
    /// </summary>
    [Export] public bool SpawnForTeamOnly { get; set; } = false;
    
    /// <summary>
    /// when true, the spawning and despawning will be handled by server, and nodes authority will be the server too.
    /// </summary>
    [Export] public bool HandleByServer { get; set; } = false;
    
    /// <summary>
    /// A properties list that should be replicated for other players during spawning, eg: "velocity", "motion_mode" and "Sprite2D:scale" in case of nested ndoes.
    /// </summary>
    [Export] public Array<string> AdditionalProps { get; set; } = new();

    public override void _Ready()
    {
        if (AutoSpawn && HandleByServer && Multiplayer.IsServer())
        {
#if MATTOHA_SERVER
            MattohaSystem.Instance.Server.SpawnNode(
                MattohaSystem.Instance.GenerateNodePayloadData(GetParent(), AdditionalProps),
                MattohaSystem.ExtractLobbyId(GetParent().GetPath())
            );
#endif
        }
        else if (AutoSpawn && !HandleByServer && !Multiplayer.IsServer() && Multiplayer.GetUniqueId() == GetMultiplayerAuthority())
        {
#if MATTOHA_CLIENT
            MattohaSystem.Instance.Client.SpawnNode(GetParent(), SpawnForTeamOnly, AdditionalProps);
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
            else if (!HandleByServer && !Multiplayer.IsServer() && Multiplayer.GetUniqueId() == GetMultiplayerAuthority())
            {
#if MATTOHA_CLIENT
                MattohaSystem.Instance.Client.DespawnNode(GetParent());
#endif
            }
        }
        base._ExitTree();
    }
}