using Godot;

namespace MattohaLobbySystem.Core.Models;
public class MattohaSpawnNodeInfo
{
	public int OwnerId { get; set; }
	public string? NodeName { get; set; }
	public string? ParentPath { get; set; }
	public string? SceneFile { get; set; }
	public Variant Position { get; set; }
	public Variant Rotation { get; set; }

	public override string ToString()
	{
		return $"SpawnInfo({OwnerId}, {NodeName}, {ParentPath}, {NodeName}, {SceneFile}, {Position}, {Rotation})";
	}
}
