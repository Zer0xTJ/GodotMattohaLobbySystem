using Godot;
using Godot.Collections;
using Mattoha.Nodes;

namespace Mattoha.Demo;
public partial class CreateLobby : Control
{
	private LineEdit _lineEdit;
	public override void _Ready()
	{
		MattohaSystem.Instance.Client.CreateLobbySucceed += OnCreateLobby;
		_lineEdit = GetNode<LineEdit>("%LobbyNameLineEdit");
		base._Ready();
	}

	private void OnCreateLobby(Dictionary<string, Variant> lobbyData)
	{
		GD.Print("Lobby created: ", lobbyData);
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/lobby.tscn");
	}

	public void OnCreateLobbyButtonPressed()
	{
		var lobbyData = new Dictionary<string, Variant>
		{
			{ "Password", "123123" },
			{ "Name", _lineEdit.Text },
			{ "PrivateProps", new Array<string> { "Password" } }
		};
		MattohaSystem.Instance.Client.CreateLobby(lobbyData, "res://csharp_demo_example/scenes/game.tscn");
	}

	public override void _ExitTree()
	{

		MattohaSystem.Instance.Client.CreateLobbySucceed -= OnCreateLobby;
		base._ExitTree();
	}
}
