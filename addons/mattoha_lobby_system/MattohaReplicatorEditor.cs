using Godot;
using Godot.Collections;
using MattohaLobbySystem.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace MattohaLobbySystem.Editors;

#if TOOLS
[Tool]
public partial class MattohaReplicatorEditor : EditorProperty
{
	private Button _propertyControl = new();
	private Array<MattohaReplicationItem> _currentValue = new();
	private Node? _rootNode;
	private List<int> _usageToIgnore = new() { 128, 10, 64, 256 };
	private MattohaLobbySystemPlugin _plugin;

	public MattohaReplicatorEditor(MattohaLobbySystemPlugin plugin)
	{
		_plugin = plugin;
		AddChild(_propertyControl);
		_propertyControl.Text = "Edit Items In Inspector";
		AddFocusable(_propertyControl);
		_propertyControl.Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		LoadCurrentValue();
		BuildViewer();
	}

	private void BuildViewer()
	{
		_plugin!.ShowViewer(InitReplicationItems(), _rootNode!.Name);
		_plugin.Viewer!.ReplicationItemChanged += OnReplicationItemChanged;
	}


	private void OnReplicationItemChanged(MattohaReplicationItem item)
	{
		var foundItem = _currentValue.FirstOrDefault(x => x.NodePath == item.NodePath && x.PropertyPath == item.PropertyPath);
		if (foundItem != null)
		{
			foundItem.IsActive = item.IsActive;
			foundItem.IsSmooth = item.IsSmooth;
			foundItem.IsTeamOnly = item.IsTeamOnly;
			foundItem.SmoothTime = item.SmoothTime;

			if (!foundItem.IsActive)
			{
				_currentValue.Remove(foundItem);
			}
		}
		else if (item.IsActive)
		{
			_currentValue.Add(item);
		}
		GetEditedObject().Set(GetEditedProperty(), _currentValue);
	}


	private Array<MattohaReplicationItem> InitReplicationItems()
	{
		Array<MattohaReplicationItem> items = new();
		var nodesToReplicate = GetReplicationNodes();
		foreach (var node in nodesToReplicate)
		{
			foreach (var property in node.GetPropertyList())
			{
				if (_usageToIgnore.Contains(property["usage"].AsInt32()))
				{
					continue;
				}
				items.Add(GenerateReplicationItem(node, property));
			}
		}
		return items;
	}

	private MattohaReplicationItem GenerateReplicationItem(Node node, Dictionary propertyData)
	{
		var foundItem = _currentValue.FirstOrDefault(x => x.NodePath == GetRealPath(node.GetPath().ToString()) && x.PropertyPath == propertyData["name"].ToString());
		var item = new MattohaReplicationItem
		{
			NodePath = GetRealPath(node.GetPath().ToString()),
			PropertyPath = propertyData["name"].ToString(),
			IsActive = foundItem?.IsActive ?? false,
			IsTeamOnly = foundItem?.IsTeamOnly ?? false,
			IsSmooth = foundItem?.IsSmooth ?? true,
			SmoothTime = foundItem?.SmoothTime ?? 0.05f,
		};
		return item;
	}

	private Array<Node> GetReplicationNodes(Node? node = null)
	{
		var replicatorNodeName = ((Node)GetEditedObject()).Name.ToString().Trim();
		Array<Node> nodes = new();
		node ??= _rootNode;
		if (node!.Name.ToString().Trim() != replicatorNodeName)
		{
			nodes.Add(node);
		}
		foreach (var child in node.GetChildren())
		{
			if (child.Name.ToString().Trim() != replicatorNodeName)
			{
				nodes.Add(child);
			}
			if (child.GetChildCount() > 0)
			{
				nodes.AddRange(GetReplicationNodes(child));
			}
		}
		return nodes;
	}

	private string GetRealPath(string path)
	{
		var realPath = "";
		var parts = path.Split("/");
		var startCounting = false;
		for (var i = 0; i < parts.Length; i++)
		{
			var part = parts[i];
			if (part == _rootNode!.Name)
			{
				startCounting = true;
			}
			if (startCounting)
			{
				if (i < parts.Length - 1)
				{
					realPath += part + "/";
				}
				else
				{
					realPath += part;
				}
			}
		}

		return realPath.Replace(_rootNode!.Name, "..");
	}

	private void LoadCurrentValue()
	{
		_rootNode = ((Node)GetEditedObject()).GetParent();
		_currentValue.Clear();
		var replicationItems = (Array<Resource>)((Node)GetEditedObject()).Get("ReplicationItems");
		foreach (var replicationItem in replicationItems)
		{
			_currentValue.Add(new MattohaReplicationItem
			{
				NodePath = (string)replicationItem.Get("NodePath"),
				PropertyPath = (string)replicationItem.Get("PropertyPath"),
				IsActive = (bool)replicationItem.Get("IsActive"),
				IsSmooth = (bool)replicationItem.Get("IsSmooth"),
				IsTeamOnly = (bool)replicationItem.Get("IsTeamOnly"),
				SmoothTime = (float)replicationItem.Get("SmoothTime"),
			});
		}

	}
}
#endif
