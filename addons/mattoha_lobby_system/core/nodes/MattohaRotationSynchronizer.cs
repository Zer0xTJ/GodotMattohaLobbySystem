using Godot;


namespace Mattoha.Nodes;
public partial class MattohaRotationSynchronizer : Node
{
	[Export] public float InterpolationTime { get; set; } = 0.05f;

	private Variant _rot;
	private Node _parent;


	public override void _EnterTree()
	{
		_parent = GetParent<Node>();
		_rot = _parent.Get("rotation");
		if (!MattohaSystem.Instance.IsNodeOwner(this))
		{
			// this will be called from unonwer players so they can sync intial rotation
			RequestRotation();
		}
		base._EnterTree();
	}


	public override void _Process(double delta)
	{
		if (MattohaSystem.Instance.IsNodeOwner(this) && MattohaSystem.Instance.Client.CanReplicate)
		{
			if (!Equals(_rot, _parent.Get("position")))
			{
				SendRotation();
			}

		}
		else
		{
			if (_parent is Node2D)
			{
				var rot = _rot.As<float>();
				(_parent as Node2D).Rotation = Mathf.Lerp(rot, (_parent as Node2D).Rotation, InterpolationTime);
			}
			else if (_parent is Node3D)
			{ 
				var rot = _rot.AsVector3();
				(_parent as Node3D).Rotation = (_parent as Node3D).Rotation.Lerp(rot, InterpolationTime);
			}
		}
		base._Process(delta);
	}


	void RequestRotation()
	{
		RpcId(GetMultiplayerAuthority(), nameof(RpcRequestRotation));
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	void RpcRequestRotation()
	{
		SendRotation();
	}


	public void SendRotation()
	{
		_rot = _parent.Get("position");
		var playersIds = MattohaSystem.Instance.Client.GetLobbyPlayersIds();
		foreach (var id in playersIds)
		{
			if (id != Multiplayer.GetUniqueId())
			{
				RpcId(id, nameof(RpcRotation), _parent.Get("position"));
			}
		}
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	void RpcRotation(Variant position)
	{
		_rot = position;
	}
}
