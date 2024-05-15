using Godot;

namespace MattohaLobbySystem.Core.Models;
public partial class MattohaReplicationItem : Resource
{
	[Export] public string? NodePath { get; set; }
	[Export] public string? PropertyPath { get; set; }
	[Export] public bool IsActive { get; set; } = false;
	[Export] public bool IsTeamOnly { get; set; } = false;
	[Export] public bool IsSmooth { get; set; } = true;
	[Export] public float SmoothTime { get; set; } = 0.05f;

	public override string ToString()
	{
		return $"ReplicationItem({NodePath}, {PropertyPath}, {IsActive}, {IsTeamOnly}, {IsSmooth}, {SmoothTime})";
	}
}
