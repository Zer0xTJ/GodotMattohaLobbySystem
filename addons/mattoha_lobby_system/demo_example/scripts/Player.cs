using Godot;
using System;

namespace MattohaLobbySystem.Demo;
public partial class Player : CharacterBody2D
{
	[Export] public PackedScene? ProjectileScene;
	[Export] public float Speed = 300;
	public override void _Process(double delta)
	{
		// only owner can move node;
		if (GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			MovePlayer();
		}
		base._Process(delta);
	}

	private void MovePlayer()
	{
		var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = direction.Normalized() * Speed;
		MoveAndSlide();
	}

	private void SpawnProjectile()
	{
		if (Input.IsActionJustPressed("ui_jump"))
		{
			var instance = MyLobbyManager.System!.CreateInstanceFromPackedScene(ProjectileScene!);
		}
	}
}
