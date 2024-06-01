using Godot;
using Mattoha.Core.Utils;

namespace Mattoha.Nodes;
public partial class MattohaGameHolder : Node
{
	public override void _Ready()
	{
		if (Multiplayer.IsServer())
			return;

		MattohaSystem.Instance.Client.LoadLobbyPlayers();
		MattohaSystem.Instance.Client.SpawnLobbyNodes();
		AddGameScene();
		base._Ready();
	}

	private void AddGameScene()
	{
		var sceneFile = MattohaSystem.Instance.Client.CurrentLobby[MattohaLobbyKeys.LobbySceneFile].ToString();
		var gameScene = GD.Load<PackedScene>(sceneFile).Instantiate();
		gameScene.Name = $"Lobby{MattohaSystem.Instance.Client.CurrentLobby[MattohaLobbyKeys.Id].AsString()}";
		AddChild(gameScene);
	}
}
