using Godot;
using Godot.Collections;
using Mattoha.Core.Utils;


namespace Mattoha.Nodes;

/// <summary>
/// A script used to detect when an CharacterBody enter the Area2D,
/// has a signal with a playerId, playerData, lobbyId and lobbyData, and a OnPickData dictionary.
/// </summary>
public partial class MattohaPickable : Node
{
	[Export] public Area2D Area2DNode { get; set; }
	[Signal] public delegate void PickedEventHandler(long playerId, Dictionary<string, Variant> playerData, int lobbyId, Dictionary<string, Variant> lobbyData, Dictionary data);
	[Export] public Dictionary OnPickData { get; set; } = new();
	[Export, ExportGroup("Server Side")] bool DetectOnServerOnly { get; set; } = true;
	[Export, ExportGroup("Client Side")] bool DetectForBodyOwnerOnly { get; set; } = false;




	public override void _Ready()
	{
		if (DetectOnServerOnly && Multiplayer.IsServer())
		{
			Area2DNode.BodyEntered += OnBodyEntered;
		}
		else if (!DetectOnServerOnly && !Multiplayer.IsServer())
		{
			Area2DNode.BodyEntered += OnBodyEntered;
		}
	}

	private void OnBodyEntered(Node2D body)
	{
#if MATTOHA_SERVER
		if (Multiplayer.IsServer() && DetectOnServerOnly)
		{
			var playerId = body.GetMultiplayerAuthority();
			var player = MattohaSystem.Instance.Server.GetPlayer(playerId);
			var lobby = MattohaSystem.Instance.Server.GetPlayerLobby(playerId);
			var lobbyId = lobby[MattohaLobbyKeys.Id].AsInt32();

			EmitSignal(SignalName.Picked, playerId, player, lobbyId, lobby, OnPickData);
		}
#endif

#if MATTOHA_CLIENT
		if (!Multiplayer.IsServer() && ((DetectForBodyOwnerOnly && MattohaSystem.Instance.IsNodeOwner(body)) || !DetectForBodyOwnerOnly))
		{
			var playerId = body.GetMultiplayerAuthority();
			var player = MattohaSystem.Instance.Client.CurrentLobbyPlayers[playerId];
			var lobby = MattohaSystem.Instance.Client.CurrentLobby;
			var lobbyId = lobby[MattohaLobbyKeys.Id].AsInt32();

			EmitSignal(SignalName.Picked, playerId, player, lobbyId, lobby, OnPickData);
		}
#endif
	}

	public override void _ExitTree()
	{
		Area2DNode.BodyEntered += OnBodyEntered;
	}
}
