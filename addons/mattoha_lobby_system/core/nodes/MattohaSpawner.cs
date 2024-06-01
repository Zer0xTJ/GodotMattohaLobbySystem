using Godot;

namespace Mattoha.Nodes;
public partial class MattohaSpawner : Node
{
	[Export] public bool AutoSpawn { get; set; } = true;
	[Export] public bool AutoDespawn { get; set; } = true;
	[Export] public bool SpawnByServer { get; set; } = true;
	[Export] public bool DespawnByServer { get; set; } = false;

	public override void _Ready()
	{
		if (GetMultiplayerAuthority() == 1)
		{
			if (AutoSpawn)
			{
				Spawn();
			}
		}
		base._Ready();
	}

	public override void _ExitTree()
	{
		if (GetMultiplayerAuthority() == 1)
		{
			if (AutoDespawn)
			{
				Despawn();
			}
		}
		base._ExitTree();
	}

	private void Spawn()
	{
		MattohaSystem.Instance.SpawnNode(GetParent(), SpawnByServer);
	}

	private void Despawn()
	{
		MattohaSystem.Instance.DespawnNode(GetParent(), DespawnByServer);
	}

}
