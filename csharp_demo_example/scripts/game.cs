using Godot;
using Mattoha.Nodes;

namespace Mattoha.Demo;
public partial class Game : Node2D
{
	public override void _Ready()
	{
		if (Multiplayer.IsServer()) return;
		SpawnPlayer();
		base._Ready();
	}

	private void SpawnPlayer()
	{
		var instance = MattohaSystem.Instance.CreateInstance("res://csharp_demo_example/scenes/player.tscn") as Node2D;
		instance.Rotation = 15;
		instance.Position = new Vector2(GD.Randi() % 100, GD.Randi() % 100);
		AddChild(instance);
		MattohaSystem.Instance.SpawnNode(instance);
	}
}
