using Godot;


namespace Mattoha.Nodes;
public partial class MattohaRotationSynchronizer : Node
{
	[Export] public float InterpolationTime { get; set; } = 0.05f;

	private Variant _rot;
	private Node _parent;


	public override void _EnterTree()
	{
		MattohaSystem.Instance.Client.SetPlayerIsInGameSucceed += OnPlayerInGame;
		_parent = GetParent<Node>();
		_rot = _parent.Get("rotation");
		base._EnterTree();
	}


	private void OnPlayerInGame(bool value)
	{
		if (!value)
			return;
		if (!MattohaSystem.Instance.IsNodeOwner(this))
		{
			// this will be called from unonwer players so they can sync intial rotation
			RequestRotation();
		}
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
		SendRotation(Multiplayer.GetRemoteSenderId());
	}


	public void SendRotation(long onlyFor = 0)
	{
		_rot = _parent.Get("position");
		var playersIds = MattohaSystem.Instance.Client.GetLobbyPlayersIds();
		if (onlyFor == 0) // 0 = send to all
		{
			foreach (var id in playersIds)
			{
				if (id != Multiplayer.GetUniqueId())
				{
					RpcId(id, nameof(RpcSendRotation), _parent.Get("position"));
				}
			}
		}
		else
		{
			RpcId(onlyFor, nameof(RpcSendRotation), _parent.Get("position"));
		}
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	void RpcSendRotation(Variant position)
	{
		_rot = position;
	}

	public override void _ExitTree()
	{
		MattohaSystem.Instance.Client.SetPlayerIsInGameSucceed -= OnPlayerInGame;
		base._ExitTree();
	}
}
