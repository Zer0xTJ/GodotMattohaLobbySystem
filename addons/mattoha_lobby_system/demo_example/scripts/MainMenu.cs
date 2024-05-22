using Godot;

namespace MattohaLobbySystem.Demo;
public partial class MainMenu : Control
{

	public override void _Ready()
	{
		MyLobbyManager.System!.Client!.ConnectedToServer += OnConnectedToServer;
		base._Ready();
	}

	public override void _ExitTree()
	{
		MyLobbyManager.System!.Client!.ConnectedToServer -= OnConnectedToServer;
		base._ExitTree();
	}

	private void OnConnectedToServer()
	{
		GetTree().ChangeSceneToFile("res://addons/mattoha_lobby_system/demo_example/scenes/user_dialog.tscn");
	}

	public void OnServerButtonPressed()
	{
		MyLobbyManager.System!.Server!.StartServer();
		QueueFree();
	}
	public void OnClientButtonPressed()
	{
		MyLobbyManager.System!.Client!.ConnectToServer();
	}
}
