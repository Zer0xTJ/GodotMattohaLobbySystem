namespace MattohaLobbySystem.Core.Models;
public class MattohaReplicationInfo
{
	public string? NodePath { get; set; }
	public string? PropertyPath { get; set; }
	public bool IsSmoothReplication { get; set; } = true;
	public float SmoothTime { get; set; } = 0.06f;
	public bool IsTeamOnly { get; set; }
}
