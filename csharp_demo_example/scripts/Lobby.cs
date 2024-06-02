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
		MattohaSystem.Instance.Client.PlayerChangedHisTeam += OnPlayerChangedHisTeam;
		MattohaSystem.Instance.Client.LoadLobbyPlayers();
		RefreshLobbyName();
		base._Ready();
	}

	private void OnPlayerChangedHisTeam(Dictionary<string, Variant> playerData)
	{
		UpdateJoinedPlayers();
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
		UpdateJoinedPlayers();
	}

	private void UpdateJoinedPlayers()
	{
		var players = MattohaSystem.Instance.Client.CurrentLobbyPlayers.Values;
		var slots = PlayersContainer.GetChildren();
		foreach (var slot in slots)
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


	public void OnJoinTeam1ButtonPressed()
	{
		MattohaSystem.Instance.Client.JoinTeam(0);
	}


	public void OnJoinTeam2ButtonPressed()
	{
		MattohaSystem.Instance.Client.JoinTeam(1);
	}

	public void OnTeamMessageButtonPressed() { }
	public void OnLobbyMessageButtonPressed() { }
	public void OnGlobalMessageButtonPressed() { }
	public void OnLeaveButtonPressed() { }

	public override void _ExitTree()
	{
		MattohaSystem.Instance.Client.StartGameSucceed += OnStartGame;
		MattohaSystem.Instance.Client.LoadLobbyPlayersSucceed += OnLoadLobbyPlayers;
		MattohaSystem.Instance.Client.NewPlayerJoined += OnNewPlayerJoiend;
		MattohaSystem.Instance.Client.SetLobbyDataSucceed += OnSetLobbyData;
		MattohaSystem.Instance.Client.PlayerChangedHisTeam += OnPlayerChangedHisTeam;
		base._ExitTree();
	}
}
