using Godot;
using Godot.Collections;

namespace Mattoha.Nodes;
public partial class MattohaServer : Node
{
	[Export] public Dictionary<long, Dictionary<string, Variant>> Players { get; set; } = new();
	[Export] public Dictionary<long, Dictionary<string, Variant>> Lobbies { get; set; } = new();
	[Export] public Dictionary<long, Array<Dictionary<string, Variant>>> SpawnedNodes { get; set; } = new();
	[Export] public Dictionary<long, Array<Dictionary<string, Variant>>> RemovedSceneNodes { get; set; } = new();
}
