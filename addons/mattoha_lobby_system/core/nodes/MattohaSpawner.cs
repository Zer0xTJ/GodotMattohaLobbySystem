using Godot;

namespace Mattoha.Nodes;
public partial class MattohaSpawner : Node
{
	[Export] public bool AutoSpawn { get; set; } = true;
	[Export] public bool AutoDespawn { get; set; } = true;
	[Export] public bool ByServer { get; set; } = true;

	public override void _Ready()
	{
		if (ByServer && Multiplayer.IsServer() && AutoSpawn)
		{
			Spawn();
		}
		else if (AutoSpawn)
		{
			Spawn();
		}
		base._Ready();
	}

	public override void _ExitTree()
	{
		if (ByServer && Multiplayer.IsServer() && AutoDespawn)
		{
			Despawn();
		}
		else if (AutoDespawn)
		{
			Despawn();
		}
		base._ExitTree();
	}

	private void Spawn()
	{
		MattohaSystem.Instance.SpawnNode(GetParent());
	}

	private void Despawn()
	{
		MattohaSystem.Instance.DespawnNode(GetParent());
	}

}
