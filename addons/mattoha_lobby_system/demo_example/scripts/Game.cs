using Godot;


namespace MattohaLobbySystem.Demo;
public partial class Game : Node2D
{
	[Export] PackedScene? PlayerScene { get; set; }
	public override void _Ready()
	{
		base._Ready();
		SpawnPlayer();
		MyLobbyManager.System?.Client?.SpawnAvailableNodes();
		MyLobbyManager.System?.Client?.DespawnRemovedSceneNodes();
	}

	private void SpawnPlayer()
	{
		var player = MyLobbyManager.System?.CreateInstanceFromPackedScene(PlayerScene!);
		AddChild(player);
		MyLobbyManager.System?.Client?.SpawnNode(player!);
	}
}
