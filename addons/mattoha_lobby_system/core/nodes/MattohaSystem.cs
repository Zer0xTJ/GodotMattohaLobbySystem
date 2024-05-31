
using Godot;

namespace Mattoha.Nodes;
public partial class MattohaSystem : Node
{
    [Export] public string ServerAddress { get; set; }
    [Export] public int ServerPort { get; set; }
}