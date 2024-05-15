using Godot;
using MattohaLobbySystem.Core.Nodes;
using MattohaLobbySystem.Core.Utils;
using MattohaLobbySystem.Demo.Models;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace MattohaLobbySystem.Demo;
public partial class Lobbies : Control
{
	[Export] PackedScene? LobbySlot { get; set; }
	[Export] VBoxContainer? LobbiesContainer { get; set; }

	public override void _Ready()
	{
		MyLobbyManager.System!.Client!.AvailableLobbiesRefreshed += OnLobbiesRefreshed;
		MyLobbyManager.System!.Client!.JoinLobbySucceed += OnJoinLobby;
		MyLobbyManager.System!.Client.LoadAvailableLobbies();
		base._Ready();
	}

	public override void _ExitTree()
	{
		MyLobbyManager.System!.Client!.AvailableLobbiesRefreshed -= OnLobbiesRefreshed;
		MyLobbyManager.System!.Client!.JoinLobbySucceed -= OnJoinLobby;
		base._ExitTree();
	}

	private void OnJoinLobby(MattohaSignal<JsonObject> lobby)
	{
		if (MattohaUtils.Deserialize<LobbyModel>(lobby.Value!)!.IsGameStarted)
		{
			GetTree().ChangeSceneToFile("res://addons/mattoha_lobby_system/demo_example/scenes/game.tscn");
		}
		else
		{
			GetTree().ChangeSceneToFile("res://addons/mattoha_lobby_system/demo_example/scenes/lobby.tscn");
		}
	}


	private void OnLobbiesRefreshed(MattohaSignal<List<JsonObject>> lobbies)
	{
		foreach (var item in LobbiesContainer!.GetChildren())
		{
			item.QueueFree();
		}

		foreach (var item in lobbies.Value!)
		{
			var slot = LobbySlot!.Instantiate<LobbySlot>();
			slot.Lobby = MattohaUtils.Deserialize<LobbyModel>(item);
			LobbiesContainer.AddChild(slot);
		}
	}

	public void OnCreateLobbyButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://addons/mattoha_lobby_system/demo_example/scenes/create_lobby.tscn");
	}
}
