using Godot;

namespace Mattoha.Nodes;
public partial class MattohaClient : Node
{
	[Signal] public delegate void ConnectedToServerEventHandler();

	public override void _Ready()
	{
		Multiplayer.ConnectedToServer += () => EmitSignal(SignalName.ConnectedToServer);
		base._Ready();
	}
}
