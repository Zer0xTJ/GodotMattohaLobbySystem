using MattohaLobbySystem.Core.Models;
using MattohaLobbySystem.Core.Utils;

namespace MattohaLobbySystem.Core.Nodes;
public partial class MattohaSpawnListenerBase : MattohaSystemFinder
{

	public override void _EnterTree()
	{
		base._EnterTree();
		GetMattohaSystem()!.Client!.NodeSpawnRequested += OnNodeSpawnRequested;
		GetMattohaSystem()!.Client!.NodeDespawnRequested += OnNodeDespawnRequested;
	}

	private void OnNodeDespawnRequested(MattohaSignal<string> nodePath)
	{
		GetNode(nodePath.Value!).QueueFree();
	}

	private void OnNodeSpawnRequested(MattohaSignal<MattohaSpawnNodeInfo> nodeInfo)
	{
		GetMattohaSystem()!.SpawnNodeFromNodeInfo(nodeInfo!.Value!);

	}

	public override void _ExitTree()
	{

		GetMattohaSystem()!.Client!.NodeSpawnRequested -= OnNodeSpawnRequested;
		GetMattohaSystem()!.Client!.NodeDespawnRequested -= OnNodeDespawnRequested;
		base._ExitTree();
	}
}
