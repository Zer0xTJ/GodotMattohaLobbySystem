using Godot;
using Mattoha.Nodes;

namespace Mattoha.Demo;
public partial class Game : Node2D
{
	[Export] public PackedScene PlayerScene { get; set; }


	public override void _Ready()
	{
		SpawnPlayer();
		base._Ready();
	}

	private void SpawnPlayer()
	{
		var instance = MattohaSystem.Instance.CreateInstance(PlayerScene) as Node2D;
		instance.Rotation = 15;
		MattohaSystem.Instance.SpawnNode(instance);
	}
}
