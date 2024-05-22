using Godot;
using MattohaLobbySystem.Core.Models;
using MattohaLobbySystem.Core.Utils;
using System.Text.Json.Nodes;

namespace MattohaLobbySystem.Core.Nodes;
public partial class MattohaReplicateListenerBase : MattohaSystemFinder
{

	/// <summary>
	/// Used by replicator, to replicate data for all lobby players.
	/// </summary>
	/// <param name="replicationInfo">MattohaReplicationInfo object</param>
	/// <param name="propertyValue">Property value</param>
	public void ReplicateData(MattohaReplicationInfo replicationInfo, Variant propertyValue)
	{
		var currentPlayerData = GetMattohaSystem()?.Client?.GetCurrentPlayerData<JsonObject>();

		foreach (var player in GetMattohaSystem()?.Client?.GetJoinedPlayers<JsonObject>()!.Values!)
		{
			if (Multiplayer.GetUniqueId() != player[MattohaPlayerKeys.Id]!.GetValue<int>())
			{
				if (replicationInfo.IsTeamOnly && player[MattohaPlayerKeys.TeamId]!.GetValue<int>() == currentPlayerData![MattohaPlayerKeys.TeamId]!.GetValue<int>())
				{
					RpcId(player[MattohaPlayerKeys.Id]!.GetValue<int>(), nameof(RpcReplicateData), MattohaUtils.Serialize(replicationInfo), propertyValue);
				}
				else if (!replicationInfo.IsTeamOnly)
				{
					RpcId(player[MattohaPlayerKeys.Id]!.GetValue<int>(), nameof(RpcReplicateData), MattohaUtils.Serialize(replicationInfo), propertyValue);
				}
			}
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void RpcReplicateData(string jsonReplicationInfo, Variant propertyValue)
	{
		var replicationInfo = MattohaUtils.Deserialize<MattohaReplicationInfo>(jsonReplicationInfo);
		if (replicationInfo == null)
			return;
		if (GetTree().Root.HasNode(replicationInfo.NodePath))
		{
			var node = GetNode(replicationInfo.NodePath);
			if (node.GetMultiplayerAuthority() != Multiplayer.GetUniqueId())
			{
				if (replicationInfo.IsSmoothReplication)
				{
					var tween = CreateTween();
					tween.TweenProperty(node, replicationInfo.PropertyPath!, propertyValue, replicationInfo.SmoothTime);
				}
				else
				{
					node?.Set(replicationInfo.PropertyPath!, propertyValue);
				}
			}
		}
	}
}
