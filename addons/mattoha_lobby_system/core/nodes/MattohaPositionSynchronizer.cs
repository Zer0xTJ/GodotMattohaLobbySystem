using Godot;

namespace Mattoha.Nodes;
public partial class MattohaPositionSynchronizer : Node
{
	[Export] public float InterpolationTime { get; set; } = 0.05f;

	private Variant _destinationPosition;
	private Node _parent;


	public override void _EnterTree()
	{
		_parent = GetParent<Node>();
		_destinationPosition = _parent.Get("position");
		if (!MattohaSystem.Instance.IsNodeOwner(this))
		{
			// this will be called from unonwer players so they can sync intial position
			RequestPosition();
		}
		base._EnterTree();
	}


	public override void _Process(double delta)
	{
		if (MattohaSystem.Instance.IsNodeOwner(this) &&  MattohaSystem.Instance.Client.CanReplicate)
		{
			if (!Equals(_destinationPosition, _parent.Get("position")))
			{
				SendPosition();
			}

		}
		else
		{
			if (_parent is Node2D)
			{
				var pos = _destinationPosition.AsVector2();
				(_parent as Node2D).Position = (_parent as Node2D).Position.Lerp(pos, InterpolationTime);
			}
			else if (_parent is Node3D)
			{ 
				var pos = _destinationPosition.AsVector3();
				(_parent as Node3D).Position = (_parent as Node3D).Position.Lerp(pos, InterpolationTime);
			}
		}
		base._Process(delta);
	}


	void RequestPosition()
	{
		RpcId(GetMultiplayerAuthority(), nameof(RpcRequestPosition));
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	void RpcRequestPosition()
	{
		SendPosition();
	}


	public void SendPosition()
	{
		_destinationPosition = _parent.Get("position");
		var playersIds = MattohaSystem.Instance.Client.GetLobbyPlayersIds();
		foreach (var id in playersIds)
		{
			if (id != Multiplayer.GetUniqueId())
			{
				RpcId(id, nameof(RpcPosition), _parent.Get("position"));
			}
		}
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	void RpcPosition(Variant position)
	{
		_destinationPosition = position;
	}
}
