using Godot;
using MattohaLobbySystem.Core.Nodes;

namespace MattohaLobbySystem.Demo;

public partial class MyLobbyManager : Node
{
	public static MattohaSystem? System { get; private set; }

	public override void _EnterTree()
	{
#if MATTOHA_SERVER
		GD.Print("#Is MATTOHA_SERVER : true");
#endif
#if MATTOHA_CLIENT
		GD.Print("#Is MATTOHA_CLIENT : true");
#endif
		System = (MattohaSystem)GetNode("MattohaSystem");
		base._EnterTree();
	}
}
