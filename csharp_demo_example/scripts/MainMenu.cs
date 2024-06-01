using Godot;
using Godot.Collections;
using Mattoha.Nodes;

namespace Mattoha.Demo;
public partial class MainMenu : Control
{
	public override void _Ready()
	{
		MattohaSystem.Instance.Client.PlayerRegistered += OnRegistered;
		base._Ready();
	}

	private void OnRegistered(Dictionary<string, Variant> playerData)
	{
		GD.Print("Registered as: ", playerData);
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/user_dialog.tscn");
	}

	public void StartServer()
	{
		MattohaSystem.Instance.StartServer();
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/game_holder.tscn");
	}

	public void StartClient()
	{
		MattohaSystem.Instance.StartClient();
	}



	public override void _ExitTree()
	{
		MattohaSystem.Instance.Client.PlayerRegistered -= OnRegistered;
	}
}
