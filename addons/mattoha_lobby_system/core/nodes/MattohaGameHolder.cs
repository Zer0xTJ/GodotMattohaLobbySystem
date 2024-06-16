using Godot;
using Mattoha.Core.Utils;

namespace Mattoha.Nodes;
public partial class MattohaGameHolder : Node
{

	/// <summary>
	/// when false, spawning lobby nodes should be done manually by you, when true, it will be automatically when game scene entered tree,
	/// </summary>
	[Export] public bool AutoSpawnLobbyNodes { get; set; } = true;

	/// <summary>
	/// when false, despawning removed scene nodes should be done manually by you, when true, it will be automatically when game scene entered tree,
	/// </summary>
	[Export] public bool AutoDespawnRemovedSceneNodes { get; set; } = true;


	public override void _Ready()
	{
		if (Multiplayer.IsServer())
			return;
		MattohaSystem.Instance.Client.LoadLobbyPlayers();
		AddGameScene();
		base._Ready();
	}

	private void AddGameScene()
	{
		var sceneFile = MattohaSystem.Instance.Client.CurrentLobby[MattohaLobbyKeys.LobbySceneFile].ToString();
		var gameScene = GD.Load<PackedScene>(sceneFile).Instantiate();
		gameScene.Name = $"Lobby{MattohaSystem.Instance.Client.CurrentLobby[MattohaLobbyKeys.Id].AsString()}";
		gameScene.TreeEntered += OnGameSceneTreeEntered;
		AddChild(gameScene);
	}

	private void OnGameSceneTreeEntered()
	{
		if (AutoSpawnLobbyNodes)
		{
			MattohaSystem.Instance.Client.SpawnLobbyNodes();
		}

		if (AutoDespawnRemovedSceneNodes)
		{
			MattohaSystem.Instance.Client.DespawnRemovedSceneNodes();
		}
	}
}
