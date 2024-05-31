using Godot;
using Godot.Collections;
using Mattoha.Nodes;

namespace Mattoha.Demo;
public partial class Lobbies : Control
{
	public override void _Ready()
	{
		MattohaSystem.Instance.Client.LoadAvailableLobbiesSucceed += OnLoadLobbies;
		MattohaSystem.Instance.Client.LoadAvailableLobbies();
		base._Ready();
	}

	private void OnLoadLobbies(Array<Dictionary<string, Variant>> lobbies)
	{
		GD.Print("Lobbies loaded: ", lobbies);
	}

	public void OpenCreateLobbyScene()
	{
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/create_lobby.tscn");
	}

	public override void _ExitTree()
	{
		MattohaSystem.Instance.Client.LoadAvailableLobbiesSucceed -= OnLoadLobbies;
		base._ExitTree();
	}
}
