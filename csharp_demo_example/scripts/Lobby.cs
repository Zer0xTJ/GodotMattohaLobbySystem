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
		MattohaSystem.Instance.Client.TeamMessageReceived += OnTeamMessage;
		MattohaSystem.Instance.Client.LobbyMessageReceived += OnLobbyMessage;
		MattohaSystem.Instance.Client.GlobalMessageReceived += OnGlobalMessage;
		MattohaSystem.Instance.Client.PlayerLeft += OnPlayerLeft;

		MattohaSystem.Instance.Client.LoadLobbyPlayers();
		RefreshLobbyName();
		base._Ready();
	}


	private void OnGlobalMessage(Dictionary<string, Variant> senderData, string message)
	{
		var label = new Label()
		{
			Text = $"Global({senderData[MattohaPlayerKeys.Username].AsString()}, {message})"
		};
		MessagesContainer.AddChild(label);
	}

	private void OnLobbyMessage(Dictionary<string, Variant> senderData, string message)
	{
		var label = new Label()
		{
			Text = $"Lobby({senderData[MattohaPlayerKeys.Username].AsString()}, {message})"
		};
		MessagesContainer.AddChild(label);
	}

	private void OnTeamMessage(Dictionary<string, Variant> senderData, string message)
	{
		var label = new Label()
		{
			Text = $"Team({senderData[MattohaPlayerKeys.Username].AsString()}, {message})"
		};
		MessagesContainer.AddChild(label);
	}

	private void OnPlayerLeft(Dictionary<string, Variant> playerData)
	{
		GD.Print("Player left: ", playerData);
		UpdateJoinedPlayers();
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

	public void OnTeamMessageButtonPressed()
	{

		MattohaSystem.Instance.Client.SendTeamMessage(MessageInput.Text);
		MessageInput.Text = "";
	}

	public void OnLobbyMessageButtonPressed()
	{
		MattohaSystem.Instance.Client.SendLobbyMessage(MessageInput.Text);
		MessageInput.Text = "";

	}
	public void OnGlobalMessageButtonPressed()
	{

		MattohaSystem.Instance.Client.SendGlobalMessage(MessageInput.Text);
		MessageInput.Text = "";
	}
	public void OnLeaveButtonPressed()
	{
		MattohaSystem.Instance.Client.LeaveLobby();
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/lobbies.tscn");
	}

	public override void _ExitTree()
	{
		MattohaSystem.Instance.Client.StartGameSucceed -= OnStartGame;
		MattohaSystem.Instance.Client.LoadLobbyPlayersSucceed -= OnLoadLobbyPlayers;
		MattohaSystem.Instance.Client.NewPlayerJoined -= OnNewPlayerJoiend;
		MattohaSystem.Instance.Client.SetLobbyDataSucceed -= OnSetLobbyData;
		MattohaSystem.Instance.Client.PlayerChangedHisTeam -= OnPlayerChangedHisTeam;
		MattohaSystem.Instance.Client.TeamMessageReceived -= OnTeamMessage;
		MattohaSystem.Instance.Client.LobbyMessageReceived -= OnLobbyMessage;
		MattohaSystem.Instance.Client.GlobalMessageReceived -= OnGlobalMessage;
		MattohaSystem.Instance.Client.PlayerLeft -= OnPlayerLeft;
		base._ExitTree();
	}
}
