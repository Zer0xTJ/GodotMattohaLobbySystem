using Godot;
using Godot.Collections;
using MattohaLobbySystem.Core.Models;
using MattohaLobbySystem.Core.UI;
using System;

[Tool]
public partial class MattohaReplicationViewer : Control
{
	[Signal] public delegate void ReplicationItemChangedEventHandler(MattohaReplicationItem item);

	[Export] VBoxContainer? ReplicationSlotsContainer { get; set; }
	[Export] PackedScene? ReplicationItemSlot { get; set; }
	[Export] LineEdit? SearchLineEdit { get; set; }
	[Export] CheckBox? ShowActiveOnly { get; set; }
	public Array<MattohaReplicationItem> Items { get; set; } = [];
	public string RootNodeName { get; set; } = "..";



	public override void _EnterTree()
	{
		base._EnterTree();
		BuildSlots();
	}

	public void BuildSlots()
	{
		foreach (var node in ReplicationSlotsContainer!.GetChildren())
		{
			node.QueueFree();
		}

		foreach (var item in Items)
		{
			if (ShowActiveOnly?.ButtonPressed == true && item.IsActive == false)
			{
				continue;
			}
			if (item.NodePath!.Contains(SearchLineEdit?.Text ?? "", StringComparison.CurrentCultureIgnoreCase) || item.PropertyPath!.Contains(SearchLineEdit?.Text ?? "", StringComparison.CurrentCultureIgnoreCase))
			{
				var slot = ReplicationItemSlot!.Instantiate<ReplicationItemSlot>();
				slot.ReplicationItem = item;
				slot.RootNodeName = RootNodeName;
				slot.ReplicationItemChanged += OnReplicationItemChanged;
				ReplicationSlotsContainer.AddChild(slot);
			}
		}
	}

	private void OnReplicationItemChanged(MattohaReplicationItem item)
	{
		EmitSignal(SignalName.ReplicationItemChanged, item);
	}

	public void OnSearchLineEditTextChanged(string text)
	{
		BuildSlots();
	}

	public void ShowActiveToggled(bool value)
	{
		BuildSlots();
	}
}
