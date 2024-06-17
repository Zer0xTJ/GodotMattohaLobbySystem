using Godot;
using System;

namespace Mattoha.Nodes;
public partial class MattohaPositionSynchronizer : Node
{
	[Export] public float InterpolationTime { get; set; } = 0.05f;

	private Variant _destinationPosition;
	private Node _parent;


	public override void _EnterTree()
	{
		MattohaSystem.Instance.Client.SetPlayerIsInGameSucceed += OnPlayerInGame;
		_parent = GetParent<Node>();
		_destinationPosition = _parent.Get("position");
		base._EnterTree();
	}

	private void OnPlayerInGame(bool value)
	{
		if (!value)
			return;
		if (!MattohaSystem.Instance.IsNodeOwner(this))
		{
			// this will be called from unonwer players so they can sync intial position
			RequestPosition();
		}
    }

	public override void _Process(double delta)
	{
		if (MattohaSystem.Instance.IsNodeOwner(this) && MattohaSystem.Instance.Client.CanReplicate)
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


	// todo: performance & packets issue, send for only player requested the position
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	void RpcRequestPosition()
	{
		SendPosition(Multiplayer.GetRemoteSenderId());
	}


	public void SendPosition(long onlyFor = 0)
	{
		_destinationPosition = _parent.Get("position");
		var playersIds = MattohaSystem.Instance.Client.GetLobbyPlayersIds();
		if (onlyFor == 0) // 0 = send to all
		{
			foreach (var id in playersIds)
			{
				if (id != Multiplayer.GetUniqueId())
				{
					RpcId(id, nameof(RpcSentPosition), _parent.Get("position"));
				}
			}
		}
		else
		{
			RpcId(onlyFor, nameof(RpcSentPosition), _parent.Get("position"));
		}
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	void RpcSentPosition(Variant position)
	{
		_destinationPosition = position;
	}

	public override void _ExitTree()
	{
		MattohaSystem.Instance.Client.SetPlayerIsInGameSucceed -= OnPlayerInGame;
		base._ExitTree();
	}
}
