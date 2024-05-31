using Godot;

namespace Mattoha.Demo;
public partial class Lobbies : Control
{
	public void OpenCreateLobbyScene()
	{
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/create_lobby.tscn");
	}
}
