using Godot;

namespace MattohaLobbySystem.Core.Utils;
public partial class MattohaSignal<T> : GodotObject
{
	public T? Value { get; set; }
}
