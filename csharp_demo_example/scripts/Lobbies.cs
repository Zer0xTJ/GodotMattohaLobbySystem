using Godot;
using Godot.Collections;
using Mattoha.Core.Utils;
using Mattoha.Nodes;

namespace Mattoha.Demo;
public partial class Lobbies : Control
{
	[Export] public PackedScene LobbySlot { get; set; }
	private VBoxContainer _lobbiesContainer;

	public override void _Ready()
	{
		_lobbiesContainer = GetNode<VBoxContainer>("%LobbiesContainer");
		MattohaSystem.Instance.Client.LoadAvailableLobbiesSucceed += OnLoadLobbies;
		MattohaSystem.Instance.Client.JoinLobbySucceed += OnJoinLobby;
		MattohaSystem.Instance.Client.LoadAvailableLobbies();
		base._Ready();
	}

	private void OnJoinLobby(Dictionary<string, Variant> lobbyData)
	{
		if (lobbyData[MattohaLobbyKeys.IsGameStarted].AsBool())
		{
			GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/game_holder.tscn");
		}
		else
		{
			GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/lobby.tscn");
		}
	}

	private void OnLoadLobbies(Array<Dictionary<string, Variant>> lobbies)
	{
		foreach (var slot in _lobbiesContainer.GetChildren())
		{
			slot.QueueFree();
		}

		foreach (var lobby in lobbies)
		{
			var lobbySlot = LobbySlot.Instantiate<LobbySlot>();
			lobbySlot.LobbyData = lobby;
			_lobbiesContainer.AddChild(lobbySlot);
		}
	}

	public void OpenCreateLobbyScene()
	{
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/create_lobby.tscn");
	}

	public override void _ExitTree()
	{
		MattohaSystem.Instance.Client.LoadAvailableLobbiesSucceed -= OnLoadLobbies;
		MattohaSystem.Instance.Client.JoinLobbySucceed -= OnJoinLobby;
		base._ExitTree();
	}
}
