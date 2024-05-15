using Godot;

namespace MattohaLobbySystem.Editors;


#if TOOLS
public partial class MattohaReplicatorInspector: EditorInspectorPlugin
{
	public MattohaLobbySystemPlugin? Plugin;
	
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
			AddPropertyEditor(name, new MattohaReplicatorEditor(Plugin!));
			return true;
		}

		return false;
	}

}
#endif
