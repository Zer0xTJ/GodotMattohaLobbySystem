using System;
using Godot;
using Godot.Collections;
using Mattoha.Core.Utils;
using Mattoha.Nodes;

public partial class Lobby : Control
{
	[Export] public Label LobbyNameLabel { get; set; }
	[Export] public VBoxContainer MessagesContainer { get; set; }
	[Export] public LineEdit MessageInput { get; set; }
	[Export] public VBoxContainer PlayersContainer { get; set; }

	public override void _Ready()
	{
		MattohaSystem.Instance.Client.StartGameSucceed += OnStartGame;
		MattohaSystem.Instance.Client.LoadLobbyPlayersSucceed += OnLoadLobbyPlayers;
		MattohaSystem.Instance.Client.NewPlayerJoined += OnNewPlayerJoiend;
		MattohaSystem.Instance.Client.SetLobbyDataSucceed += OnSetLobbyData;
		MattohaSystem.Instance.Client.LoadLobbyPlayers();
		RefreshLobbyName();
		base._Ready();
	}

	private void OnSetLobbyData(Dictionary<string, Variant> lobbyData)
	{
		RefreshLobbyName();
	}

	private void RefreshLobbyName()
	{
		LobbyNameLabel.Text = $"Lobby({MattohaSystem.Instance.Client.CurrentLobby[MattohaLobbyKeys.Id].AsInt64()}, {MattohaSystem.Instance.Client.CurrentLobby[MattohaLobbyKeys.Name].AsString()})";
	}

	private void OnNewPlayerJoiend(Dictionary<string, Variant> player)
	{
		var playerLabel = new Label
		{
			Text = $"Player({player[MattohaPlayerKeys.Id].AsInt64()}, {player[MattohaPlayerKeys.Username].AsString()}, {player[MattohaPlayerKeys.TeamId].AsInt32()})"
		};
		PlayersContainer.AddChild(playerLabel);
	}

	private void OnLoadLobbyPlayers(Array<Dictionary<string, Variant>> players)
	{
		GD.Print("playersssss: ", players);
		foreach (var slot in PlayersContainer.GetChildren())
		{
			slot.QueueFree();
		}
		foreach (var player in players)
		{
			var playerLabel = new Label
			{
				Text = $"Player({player[MattohaPlayerKeys.Id].AsInt64()}, {player[MattohaPlayerKeys.Username].AsString()}, {player[MattohaPlayerKeys.TeamId].AsInt32()})"
			};
			PlayersContainer.AddChild(playerLabel);
		}
	}

	private void OnStartGame(Dictionary<string, Variant> lobbyData)
	{
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/game_holder.tscn");
	}

	public void StartGame()
	{
		MattohaSystem.Instance.Client.StartGame();
	}


	public void OnRandomLobbyNamePressed()
	{
		MattohaSystem.Instance.Client.SetLobbyData(new Dictionary<string, Variant>()
		{
			{ MattohaLobbyKeys.Name, $"{GD.Randi() % 1000}" }
		});
	}


	public void OnJoinTeam1ButtonPressed() { }
	public void OnJoinTeam2ButtonPressed() { }
	public void OnTeamMessageButtonPressed() { }
	public void OnLobbyMessageButtonPressed() { }
	public void OnGlobalMessageButtonPressed() { }
	public void OnLeaveButtonPressed() { }

	public override void _ExitTree()
	{
		MattohaSystem.Instance.Client.StartGameSucceed -= OnStartGame;
		MattohaSystem.Instance.Client.LoadLobbyPlayersSucceed -= OnLoadLobbyPlayers;
		base._ExitTree();
	}
}
