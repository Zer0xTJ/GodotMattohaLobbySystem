using Godot;
using MattohaLobbySystem.Core.Utils;
using MattohaLobbySystem.Demo.Models;
using System.Text.Json.Nodes;

namespace MattohaLobbySystem.Demo;
public partial class CreateLobby : Control
{
	[Export] LineEdit? LobbyNameLineEdit { get; set; }

	public override void _Ready()
	{
		MyLobbyManager.System!.Client!.NewLobbyCreated += OnNewLobbyCreated;
		base._Ready();
	}

	public override void _ExitTree()
	{
		MyLobbyManager.System!.Client!.NewLobbyCreated -= OnNewLobbyCreated;
		base._ExitTree();
	}

	private void OnNewLobbyCreated(MattohaSignal<JsonObject> lobby)
	{
		GetTree().ChangeSceneToFile("res://addons/mattoha_lobby_system/demo_example/scenes/lobby.tscn");
	}

	public void OnCreateLobbyButtonPressed()
	{
		var lobby = new LobbyModel
		{
			Name = LobbyNameLineEdit!.Text,
			MaxPlayers = (int)(GD.Randi() % 10 + 4)
		};

		MyLobbyManager.System!.Client!.CreateLobby(lobby.ToDict());

	}
}
