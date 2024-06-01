using Godot;

namespace Mattoha.Demo;
public partial class Coin : Area2D
{

	public void OnBodyEntered(Node2D body)
	{
		QueueFree();
	}
}
