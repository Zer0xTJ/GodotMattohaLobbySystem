using Godot;
using MattohaLobbySystem.Core.Models;

namespace MattohaLobbySystem.Core.Nodes;

public partial class MattohaSystem : Node
{
	/// <summary>
	/// The Default Host IP that server will run on, and clients connect to.
	/// </summary>
	[ExportGroup("Configuration")]
	[Export] public string DefaultHost { get; set; } = "127.0.0.1";

	/// <summary>
	/// The Default port that server is running on.
	/// </summary>
	[ExportGroup("Configuration")]
	[Export] public int DefaultPort { get; set; } = 7001;

	/// <summary>
	/// Maximum channels for RPCs.
	/// </summary>
	[ExportGroup("Configuration")]
	[Export] public int MaxRpcChannels { get; set; } = 4;

	/// <summary>
	/// Maximum players allowed to join to tha server at same time.
	/// </summary>
	[ExportGroup("Configuration")]
	[Export(PropertyHint.Range, "1, 4095")] public int MaxPlayersInServer { get; set; } = 500;

	/// <summary>
	/// Max players count allowd in lobbies, owner can change the number, but not more than MaxPlayersPerLobby.
	/// </summary>
	[ExportGroup("Configuration")]
	[Export(PropertyHint.Range, "2, 100")] public int MaxPlayersPerLobby { get; set; } = 10;


	/// <summary>
	/// If true, any change on lobby data will be synced for all connected players on "available lobbies"
	/// </summary>
	[ExportGroup("Configuration")]
	[Export] public bool AutoRefreshAvailableLobbies = true;


	/// <summary>
	/// If true, nodes spawned from player will be despawned when a player leave the server.
	/// </summary>
	[ExportGroup("Configuration")]
	[Export] public bool AutoDespawnPlayerNodesOnLeave = true;


	/// <summary>
	/// The MattohaServer node, It MUST be a child of this node.
	/// </summary>
	[ExportGroup("Core Nodes")]
	[Export] public MattohaServer? Server { get; set; }


	/// <summary>
	/// The MattohaClient node, It MUST be a child of this node.
	/// </summary>
	[ExportGroup("Core Nodes")]
	[Export] public MattohaClient? Client { get; set; }


	/// <summary>
	/// Create a scene instance from scene file with a unique name and assign authority to owner.
	/// </summary>
	/// <param name="file">file path for scene.</param>
	/// <returns>The created node instance.</returns>
	public Node CreateInstanceFromSceneFile(string file)
	{
		var packedScene = GD.Load<PackedScene>(file);
		var instance = packedScene.Instantiate();
		var ownerId = Multiplayer.GetUniqueId();
		instance.Name += $"_{GD.Randi() % 10000 + 99999}_{ownerId}";
		instance.SetMultiplayerAuthority(ownerId);
		return instance;
	}


	/// <summary>
	/// Create a scene instance from PackedScene with a unique name and assign authority to owner.
	/// </summary>
	/// <param name="file">file path for scene.</param>
	/// <returns>The created node instance.</returns>
	public Node CreateInstanceFromPackedScene(PackedScene scene)
	{
		var instance = scene.Instantiate();
		var ownerId = Multiplayer.GetUniqueId();
		instance.Name += $"_{GD.Randi() % 10000 + 99999}_{ownerId}";
		instance.SetMultiplayerAuthority(ownerId);
		return instance;
	}

	/// <summary>
	/// Create an instance from node info object
	/// </summary>
	/// <param name="nodeInfo">MattohaSpawnNodeInfo object.</param>
	/// <returns>created node.</returns>
	public Node? SpawnNodeFromNodeInfo(MattohaSpawnNodeInfo nodeInfo)
	{
		if (!GetTree().Root.HasNode($"{nodeInfo.ParentPath}/{nodeInfo.NodeName}"))
		{
			var instance = GD.Load<PackedScene>(nodeInfo.SceneFile).Instantiate();
			var parent = GetNode(nodeInfo.ParentPath);
			instance.Name = nodeInfo.NodeName;
			instance.SetMultiplayerAuthority(nodeInfo.OwnerId);
			instance.Set("position", nodeInfo.Position);
			instance.Set("rotation", nodeInfo.Rotation);
			parent.AddChild(instance);
			return instance;
		}
		return null;
	}
}
