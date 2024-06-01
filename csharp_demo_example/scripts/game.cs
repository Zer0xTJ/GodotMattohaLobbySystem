using Godot;
using Mattoha.Nodes;

namespace Mattoha.Demo;
public partial class Game : Node2D
{
	public override void _Ready()
	{
		if (Multiplayer.IsServer()) return;
		foreach (var peer in MattohaSystem.Instance.Client.GetLobbyPlayerIds())
		{
			RpcId(peer, nameof(SpawnPlayer));
		}
		base._Ready();
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void SpawnPlayer()
	{
		var instance = MattohaSystem.Instance.CreateInstance("res://csharp_demo_example/scenes/player.tscn", Multiplayer.GetRemoteSenderId()) as Node2D;
		instance.Rotation = 15;
		instance.Position = new Vector2(GD.Randi() % 100, GD.Randi() % 100);
		AddChild(instance);
	}

}
