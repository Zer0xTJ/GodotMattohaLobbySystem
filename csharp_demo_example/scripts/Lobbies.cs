using Godot;
using Godot.Collections;
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
		GD.Print("Lobby Joined: ", lobbyData);
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/lobby.tscn");
	}

	private void OnLoadLobbies(Array<Dictionary<string, Variant>> lobbies)
	{
		GD.Print("Lobbies loaded: ", lobbies);
		foreach(var slot in _lobbiesContainer.GetChildren())
		{
			slot.QueueFree();
		}

		foreach(var lobby in lobbies)
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
