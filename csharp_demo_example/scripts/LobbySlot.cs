using Godot;
using Godot.Collections;
using Mattoha.Nodes;
using Mattoha.Core.Utils;


namespace Mattoha.Demo;
public partial class LobbySlot : HBoxContainer
{

	public Dictionary<string, Variant> LobbyData { get; set; }

	private Label _lobbyNameLabel;
	private Label _playersCountLabel;

	public override void _Ready()
	{
		_lobbyNameLabel = GetNode<Label>("%LobbyNameLabel");
		_playersCountLabel = GetNode<Label>("%PlayersCountLabel");

		_lobbyNameLabel.Text = LobbyData["Name"].AsString();
		_playersCountLabel.Text = $"{LobbyData[MattohaLobbyKeys.PlayersCount]} / {LobbyData[MattohaLobbyKeys.MaxPlayers]}";
		base._Ready();
	}

	public void JoinLobby()
	{
		MattohaSystem.Instance.Client.JoinLobby(LobbyData[MattohaLobbyKeys.Id].AsInt32());
	}
}
