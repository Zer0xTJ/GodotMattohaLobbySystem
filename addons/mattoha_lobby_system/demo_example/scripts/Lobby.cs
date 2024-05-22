using Godot;
using MattohaLobbySystem.Core;
using MattohaLobbySystem.Core.Nodes;
using MattohaLobbySystem.Core.Utils;
using MattohaLobbySystem.Demo.Models;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace MattohaLobbySystem.Demo;
public partial class Lobby : Control
{

	[Export] Label? LobbyNameLabel { get; set; }
	[Export] VBoxContainer? MessagesContainer { get; set; }
	[Export] LineEdit? MessageLineEdit { get; set; }
	[Export] VBoxContainer? PlayersContainer { get; set; }


	public override void _Ready()
	{
		base._Ready();
		MyLobbyManager.System!.Client!.NewPlayerJoinedLobby += OnNewPlayerJoined;
		MyLobbyManager.System!.Client!.JoinedLobbyRefreshed += OnJoinedLobbyRefreshed;
		MyLobbyManager.System!.Client!.JoinedPlayersRefreshed += OnJoinedPlayersRefreshed;
		MyLobbyManager.System!.Client!.TeamMessageReceived += OnTeamMessageReceived;
		MyLobbyManager.System!.Client!.LobbyMessageReceived += OnLobbyMessageReceived;
		MyLobbyManager.System!.Client!.GlobalMessageReceived += OnGlobalMessageReceived;

		MyLobbyManager.System!.Client!.GameStarted += OnGameStarted;
		MyLobbyManager.System!.Client!.PlayerLeftLobby += OnPlayerLeftLobby;

		MyLobbyManager.System!.Client.RefreshJoinedLobby();
		MyLobbyManager.System!.Client.RefreshJoinedPlayers();

	}

	private void OnPlayerLeftLobby(MattohaSignal<JsonObject> player)
	{
	}

	private void OnGameStarted()
	{
		GetTree().ChangeSceneToFile("res://addons/mattoha_lobby_system/demo_example/scenes/game.tscn");
	}

	private void OnGlobalMessageReceived(MattohaSignal<string> message, MattohaSignal<JsonObject> player)
	{
		var playerModel = MattohaUtils.Deserialize<PlayerModel>(player.Value!);
		var label = new Label { Text = $"[Global] {playerModel!.Username} : {message.Value}" };
		MessagesContainer?.AddChild(label);
	}

	private void OnLobbyMessageReceived(MattohaSignal<string> message, MattohaSignal<JsonObject> player)
	{
		var playerModel = MattohaUtils.Deserialize<PlayerModel>(player.Value!);
		var label = new Label { Text = $"[Lobby] {playerModel!.Username} : {message.Value}" };
		MessagesContainer?.AddChild(label);
	}

	private void OnTeamMessageReceived(MattohaSignal<string> message, MattohaSignal<JsonObject> player)
	{
		var playerModel = MattohaUtils.Deserialize<PlayerModel>(player.Value!);
		var label = new Label { Text = $"[Team] {playerModel!.Username} : {message.Value}" };
		MessagesContainer?.AddChild(label);
	}

	private void OnJoinedPlayersRefreshed(MattohaSignal<Dictionary<long, JsonObject>> players)
	{

		foreach (var node in PlayersContainer!.GetChildren())
		{
			node.QueueFree();
		}

		foreach (var pl in players.Value!.Values)
		{
			var slot = new Label
			{
				Text = MattohaUtils.Deserialize<PlayerModel>(pl)!.ToString()
			};
			PlayersContainer.AddChild(slot);
		}
	}

	private void OnJoinedLobbyRefreshed(MattohaSignal<JsonObject> lobbyObj)
	{
		var lobby = MattohaUtils.Deserialize<LobbyModel>(lobbyObj.Value!);
		LobbyNameLabel!.Text = $"{lobby!.Name} ({lobby.PlayersCount})";
	}

	private void OnNewPlayerJoined(MattohaSignal<JsonObject> player)
	{
		GD.Print(player.Value);
	}


	public void OnRandomNameButtonPressed()
	{
		var lobby = MyLobbyManager.System!.Client!.GetCurrentLobbyData<LobbyModel>();
		lobby!.Name = $"{GD.Randi() % 9999 + 1000}";
		MyLobbyManager.System!.Client!.SetLobbyData(lobby.ToDict());
	}

	public void OnRefreshLobbyButtonPressed()
	{
		MyLobbyManager.System?.Client?.RefreshJoinedLobby();
	}


	public void OnTeam1ButtonPressed()
	{
		var player = MyLobbyManager.System!.Client!.GetCurrentPlayerData<PlayerModel>();
		player!.TeamId = 1;
		MyLobbyManager.System?.Client?.SetPlayerData(player.ToDict());

	}

	public void OnTeam2ButtonPressed()
	{
		var player = MyLobbyManager.System!.Client!.GetCurrentPlayerData<PlayerModel>();
		player!.TeamId = 2;
		MyLobbyManager.System?.Client?.SetPlayerData(player.ToDict());
	}

	public void OnRefreshPlayersButtonPressed()
	{
		MyLobbyManager.System?.Client?.RefreshJoinedPlayers();
	}


	public void OnTeamMessageButtonPressed()
	{
		MyLobbyManager.System?.Client?.SendTeamMessage(MessageLineEdit!.Text);
		MessageLineEdit!.Text = "";
	}


	public void OnLobbyMessageButtonPressed()
	{
		MyLobbyManager.System?.Client?.SendLobbyMessage(MessageLineEdit!.Text);
		MessageLineEdit!.Text = "";
	}

	public void OnGlobalMessageButtonPressed()
	{
		MyLobbyManager.System?.Client?.SendGlobalMessage(MessageLineEdit!.Text);
		MessageLineEdit!.Text = "";
	}

	public void OnStartGameButtonPressed()
	{
		MyLobbyManager.System!.Client!.StartGame();
	}

	public void OnLeaveButtonPressed()
	{
		MyLobbyManager.System!.Client!.LeaveLobby();
		GetTree().ChangeSceneToFile("res://addons/mattoha_lobby_system/demo_example/scenes/lobbies.tscn");
	}

	public override void _ExitTree()
	{
		MyLobbyManager.System!.Client!.NewPlayerJoinedLobby -= OnNewPlayerJoined;
		MyLobbyManager.System!.Client!.JoinedLobbyRefreshed -= OnJoinedLobbyRefreshed;
		MyLobbyManager.System!.Client!.JoinedPlayersRefreshed -= OnJoinedPlayersRefreshed;
		MyLobbyManager.System!.Client!.TeamMessageReceived -= OnTeamMessageReceived;
		MyLobbyManager.System!.Client!.LobbyMessageReceived -= OnLobbyMessageReceived;
		MyLobbyManager.System!.Client!.GlobalMessageReceived -= OnGlobalMessageReceived;

		MyLobbyManager.System!.Client!.GameStarted -= OnGameStarted;
		MyLobbyManager.System!.Client!.PlayerLeftLobby -= OnPlayerLeftLobby;
		base._ExitTree();
	}

}
