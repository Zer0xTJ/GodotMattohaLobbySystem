using Godot;
using Godot.Collections;
using MattohaLobbySystem.Core.Models;

namespace MattohaLobbySystem.Core.Nodes;
public partial class MattohaReplicatorBase : MattohaSystemFinder
{

	[Export] bool ReplicationEnabled = true;
	[Export] bool AutoDespawn = true;
	[Export] float ReplicationIntervalSeconds = 0;
	[Export] public Array<MattohaReplicationItem> ReplicationItems = new();
	private float _lastReplicationTime = 0;

	public override void _EnterTree()
	{
		base._EnterTree();
		_lastReplicationTime = Time.GetTicksMsec() - (ReplicationIntervalSeconds * 1000);
	}

	public override void _ExitTree()
	{
		if (AutoDespawn)
		{
			GetMattohaSystem()!.Client!.DespawnNode(GetNode(".."));
		}
		base._ExitTree();
	}


	public override void _Process(double delta)
	{
		base._Process(delta);
		if (Time.GetTicksMsec() - _lastReplicationTime > (ReplicationIntervalSeconds * 1000) && GetMultiplayerAuthority() == Multiplayer.GetUniqueId() && ReplicationEnabled)
		{
			ReplicateData();
		}
	}

	private void ReplicateData()
	{
		foreach (var item in ReplicationItems)
		{
			var node = GetNode(item.NodePath);
			var replicateInfo = new MattohaReplicationInfo
			{
				NodePath = node.GetPath().ToString(),
				PropertyPath = item.PropertyPath,
				IsSmoothReplication = item.IsSmooth,
				SmoothTime = item.SmoothTime,
				IsTeamOnly = item.IsTeamOnly,
			};
			GetMattohaReplicateListener()!.ReplicateData(replicateInfo, node.Get(item.PropertyPath));
		}
		_lastReplicationTime = Time.GetTicksMsec();
	}

}
