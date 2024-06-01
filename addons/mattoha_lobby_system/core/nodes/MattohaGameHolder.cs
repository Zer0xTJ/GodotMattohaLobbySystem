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
		
		var sceneFile = MattohaSystem.Instance.Client.CurrentLobby[MattohaLobbyKeys.LobbySceneFile].ToString();
		AddChild(GD.Load<PackedScene>(sceneFile).Instantiate());

		base._Ready();
	}
}
