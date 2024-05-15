using Godot;
using MattohaLobbySystem.Core.Models;

namespace MattohaLobbySystem.Core.UI;

[Tool]
public partial class ReplicationItemSlot : MarginContainer
{
	[Signal] public delegate void ReplicationItemChangedEventHandler(MattohaReplicationItem item);

	[Export] public Label? NodeNameLabel { get; set; }
	[Export] public Label? PropertyPathLabel { get; set; }
	[Export] public CheckBox? IsActiveCheckBox { get; set; }
	[Export] public CheckBox? IsTeamOnlyCheckBox { get; set; }
	[Export] public CheckBox? IsSmoothCheckBox { get; set; }
	[Export] public LineEdit? SmoothTimeLineEdit { get; set; }

	public MattohaReplicationItem? ReplicationItem { get; set; }
	public string RootNodeName = "..";

	public override void _EnterTree()
	{
		base._EnterTree();
		if (ReplicationItem != null)
		{
			NodeNameLabel!.Text = ReplicationItem!.NodePath!.Replace("..", RootNodeName);
			PropertyPathLabel!.Text = ReplicationItem!.PropertyPath;
			SmoothTimeLineEdit!.Text = ReplicationItem!.SmoothTime.ToString();
			IsActiveCheckBox!.Set("button_pressed", ReplicationItem!.IsActive);
			IsSmoothCheckBox!.Set("button_pressed", ReplicationItem!.IsSmooth);
			IsTeamOnlyCheckBox!.Set("button_pressed", ReplicationItem!.IsTeamOnly);
		}
	}

	public void OnIsActiveButtonToggled(bool value)
	{
		ReplicationItem!.IsActive = value;
		EmitSignal(nameof(ReplicationItemChanged), ReplicationItem);
	}

	public void OnIsSmoothButtonToggled(bool value)
	{
		ReplicationItem!.IsSmooth = value;
		SmoothTimeLineEdit!.Editable = value;
		EmitSignal(nameof(ReplicationItemChanged), ReplicationItem);
	}

	public void OnIsTeamOnlyButtonToggled(bool value)
	{
		ReplicationItem!.IsTeamOnly = value;
		EmitSignal(nameof(ReplicationItemChanged), ReplicationItem);
	}

	public void OnSmoothTimeLineEditChanged(string value)
	{
		ReplicationItem!.SmoothTime = value.ToFloat();
		EmitSignal(nameof(ReplicationItemChanged), ReplicationItem);
	}
}
