using Godot;
using MattohaLobbySystem.Core.Utils;
using MattohaLobbySystem.Demo.Models;
using System.Text.Json.Nodes;

namespace MattohaLobbySystem.Demo;
public partial class UserDialog : Control
{
	[Export] public LineEdit? UsernameLineEdit { get; set; }

	public override void _Ready()
	{
		MyLobbyManager.System!.Client!.CurrentPlayerUpdated += OnCurrentPlayerUpdated;
		MyLobbyManager.System!.Client!.SetPlayerDataFailed += OnSetPlayerDataFailed;
		base._Ready();
	}

	private void OnSetPlayerDataFailed(MattohaSignal<string> failCause)
	{
		GD.Print("Setting player data failed: ", failCause.Value);
	}

	public override void _ExitTree()
	{
		MyLobbyManager.System!.Client!.CurrentPlayerUpdated -= OnCurrentPlayerUpdated;
		base._ExitTree();
	}

	private void OnCurrentPlayerUpdated(MattohaSignal<JsonObject> player)
	{
		GD.Print(player.Value);
		GetTree().ChangeSceneToFile("res://addons/mattoha_lobby_system/demo_example/scenes/lobbies.tscn");
	}

	public void OnContinueButtonPressed()
	{
		var player = new PlayerModel()
		{
			Username = UsernameLineEdit!.Text,
		};
		MyLobbyManager.System?.Client?.SetPlayerData(player.ToDict());
	}
}
