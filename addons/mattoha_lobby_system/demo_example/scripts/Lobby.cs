using Godot;
using Godot.Collections;
using MattohaLobbySystem.Core;
using MattohaLobbySystem.Core.Utils;
using MattohaLobbySystem.Demo.Models;
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

	private void OnPlayerLeftLobby(Dictionary<string, Variant> player)
	{
	}

	private void OnGameStarted()
	{
		GetTree().ChangeSceneToFile("res://addons/mattoha_lobby_system/demo_example/scenes/game.tscn");
	}

	private void OnGlobalMessageReceived(string message, Dictionary<string, Variant> player)
	{
		var playerModel = MattohaUtils.Deserialize<PlayerModel>(player);
		var label = new Label { Text = $"[Global] {playerModel!.Username} : {message}" };
		MessagesContainer?.AddChild(label);
	}

	private void OnLobbyMessageReceived(string message, Dictionary<string, Variant> player)
	{
		var playerModel = MattohaUtils.Deserialize<PlayerModel>(player);
		var label = new Label { Text = $"[Lobby] {playerModel!.Username} : {message}" };
		MessagesContainer?.AddChild(label);
	}

	private void OnTeamMessageReceived(string message, Dictionary<string, Variant> player)
	{
		var playerModel = MattohaUtils.Deserialize<PlayerModel>(player);
		var label = new Label { Text = $"[Team] {playerModel!.Username} : {message}" };
		MessagesContainer?.AddChild(label);
	}

	private void OnJoinedPlayersRefreshed(Dictionary<long, Dictionary<string, Variant>> players)
	{

		foreach (var node in PlayersContainer!.GetChildren())
		{
			node.QueueFree();
		}

		foreach (var pl in players.Values)
		{
			var slot = new Label
			{
				Text = MattohaUtils.Deserialize<PlayerModel>(pl)!.ToString()
			};
			PlayersContainer.AddChild(slot);
		}
	}

	private void OnJoinedLobbyRefreshed(Dictionary<string, Variant> lobbyObj)
	{
		var lobby = MattohaUtils.Deserialize<LobbyModel>(lobbyObj);
		LobbyNameLabel!.Text = $"{lobby!.Name} ({lobby.PlayersCount})";
	}

	private void OnNewPlayerJoined(Dictionary<string, Variant> player)
	{
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
