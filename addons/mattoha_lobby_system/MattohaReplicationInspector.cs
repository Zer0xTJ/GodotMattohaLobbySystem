using Godot;

namespace MattohaLobbySystem.Editors;


#if TOOLS
public partial class MattohaReplicatorInspector: EditorInspectorPlugin
{
	private MattohaLobbySystemPlugin _plugin;
	
	public MattohaReplicatorInspector(MattohaLobbySystemPlugin plugin)
	{
		_plugin = plugin;
	}

	public override bool _CanHandle(GodotObject @object)
	{
		return true;
	}

	public override bool _ParseProperty(GodotObject @object, Variant.Type type,
		string name, PropertyHint hintType, string hintString,
		PropertyUsageFlags usageFlags, bool wide)
	{
		if (name == "ReplicationItems")
		{
			AddPropertyEditor(name, new MattohaReplicatorEditor(_plugin));
			return true;
		}

		return false;
	}

}
#endif
