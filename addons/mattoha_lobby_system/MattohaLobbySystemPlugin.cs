using Godot;
using Godot.Collections;
using MattohaLobbySystem.Core.Models;
using MattohaLobbySystem.Editors;

namespace MattohaLobbySystem;
#if TOOLS
[Tool]
public partial class MattohaLobbySystemPlugin : EditorPlugin
{
	public MattohaReplicationViewer? Viewer { get; set; }
	private MattohaReplicatorInspector? _mattohaReplicationInspector;
	private EditorSelection? _selection { get; set; }

	public override void _EnterTree()
	{
		AddTypes();
		_mattohaReplicationInspector = new MattohaReplicatorInspector(this);
		AddInspectorPlugin(_mattohaReplicationInspector);
		_selection = EditorInterface.Singleton.GetSelection();
		_selection.SelectionChanged += OnSelectionChanged;
		base._EnterTree();
	}

	private void OnSelectionChanged()
	{
		RemoveViewer();
	}

	public override void _ExitTree()
	{
		RemoveTypes();
		RemoveViewer();
		RemoveInspectorPlugin(_mattohaReplicationInspector);
		_selection!.SelectionChanged -= OnSelectionChanged;
		base._ExitTree();
	}

	public void ShowViewer(Array<MattohaReplicationItem> items, string rootNodeName)
	{
		RemoveViewer();
		var scene = GD.Load<PackedScene>("res://addons/mattoha_lobby_system/core/UI/mattoha_replication_viewer.tscn");
		Viewer = scene.Instantiate<MattohaReplicationViewer>();
		Viewer.Items = items;
		Viewer.RootNodeName = rootNodeName;
		var button = AddControlToBottomPanel(Viewer, "Mattoha Viewer");
		button.Set("button_pressed", true);
	}

	public void RemoveViewer()
	{
		if (Viewer != null)
		{
			RemoveControlFromBottomPanel(Viewer);
			Viewer.QueueFree();
			Viewer = null;
		}
	}

	private void AddTypes()
	{
		AddCustomType("MattohaSystem", "Node", GD.Load<Script>("res://addons/mattoha_lobby_system/core/nodes/MattohaSystem.cs"), null);
		AddCustomType("MattohaServer", "Node", GD.Load<Script>("res://addons/mattoha_lobby_system/core/nodes/MattohaServer.cs"), null);
		AddCustomType("MattohaClient", "Node", GD.Load<Script>("res://addons/mattoha_lobby_system/core/nodes/MattohaClient.cs"), null);
		AddCustomType("MattohaSpawnListener", "Node", GD.Load<Script>("res://addons/mattoha_lobby_system/core/nodes/MattohaSpawnListener.cs"), null);
		AddCustomType("MattohaReplicator", "Node", GD.Load<Script>("res://addons/mattoha_lobby_system/core/nodes/MattohaReplicator.cs"), null);
		AddCustomType("MattohaReplicateListener", "Node", GD.Load<Script>("res://addons/mattoha_lobby_system/core/nodes/MattohaReplicateListener.cs"), null);
	}


	private void RemoveTypes()
	{
		RemoveCustomType("MattohaSystem");
		RemoveCustomType("MattohaServer");
		RemoveCustomType("MattohaClient");
		RemoveCustomType("MattohaSpawnListener");
		RemoveCustomType("MattohaReplicator");
		RemoveCustomType("MattohaReplicateListener");
	}
}
#endif
