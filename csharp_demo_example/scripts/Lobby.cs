using Godot;
using Godot.Collections;
using Mattoha.Nodes;

public partial class Lobby : Control
{

	public override void _Ready()
	{
		MattohaSystem.Instance.Client.StartGameSucceed += OnStartGame;
		base._Ready();
	}

	private void OnStartGame(Dictionary<string, Variant> lobbyData)
	{
		GD.Print("Game Started: ", lobbyData);
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/game_holder.tscn");
	}

	public void StartGame()
	{
		MattohaSystem.Instance.Client.StartGame();
	}

	public override void _ExitTree()
	{
		MattohaSystem.Instance.Client.StartGameSucceed -= OnStartGame;
		base._ExitTree();
	}
}
