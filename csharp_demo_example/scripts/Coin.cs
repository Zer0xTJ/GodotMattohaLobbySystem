using Godot;

public partial class Coin : Area2D
{

	public void BodyEntered(Node2D body)
	{
		QueueFree();
	}
}
