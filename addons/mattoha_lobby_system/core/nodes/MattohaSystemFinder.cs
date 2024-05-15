using Godot;

namespace MattohaLobbySystem.Core.Nodes;
public partial class MattohaSystemFinder : Node
{

	MattohaSystem? _system;
	MattohaReplicateListenerBase? _replicateListener;

	public override void _EnterTree()
	{
		InitSystem();
		InitReplicateListener();
		base._EnterTree();
	}
	
	/// <summary>
	/// return the MattohaSystem from node tree if found.
	/// </summary>
	/// <returns>MattohaSystem node.</returns>
	public MattohaSystem? GetMattohaSystem()
	{
		return _system;
	}
	
	/// <summary>
	/// Return the replicate listener.
	/// </summary>
	/// <returns>MattohaReplicateListener node.</returns>
	public MattohaReplicateListenerBase? GetMattohaReplicateListener()
	{
		return _replicateListener;
	}


	private void InitReplicateListener(Node? node = null)
	{
		if (_replicateListener != null)
			return;

		node ??= GetNode("/root/");

		if (node.GetChildCount() > 0)
		{
			foreach (var childNode in node.GetChildren())
			{
				InitReplicateListener(childNode);
				if (childNode is MattohaReplicateListenerBase replicateListener)
				{
					_replicateListener = replicateListener;
					break;
				}
			}
		}
		else
		{
			if (node is MattohaReplicateListenerBase replicateListener)
			{
				_replicateListener = replicateListener;
			}
		}

	}

	private void InitSystem(Node? node = null)
	{
		if (_system != null)
			return;

		node ??= GetNode("/root/");

		if (node.GetChildCount() > 0)
		{
			foreach (var childNode in node.GetChildren())
			{
				InitSystem(childNode);
				if (childNode is MattohaSystem system)
				{
					_system = system;
					break;
				}
			}
		}
		else
		{
			if (node is MattohaSystem system)
			{
				_system = system;
			}
		}

	}
}
