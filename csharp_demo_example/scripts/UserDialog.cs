using Godot;
using Godot.Collections;
using Mattoha.Nodes;

namespace Mattoha.Core;
public partial class UserDialog : Control
{
	private LineEdit _usernameInput; 
	public override void _Ready()
	{
		_usernameInput = GetNode<LineEdit>("%UsernameLineEdit");
		MattohaSystem.Instance.Client.SetPlayerDataSucceed += OnSetPlayerData;
		base._Ready();
	}

	private void OnSetPlayerData(Dictionary<string, Variant> playerData)
	{
		GD.Print("New Player data: ", playerData);
		GetTree().ChangeSceneToFile("res://csharp_demo_example/scenes/lobbies.tscn");
	}

	public void OnContinueButtonPressed()
	{
		var data = new Dictionary<string, Variant>()
		{
			{ "Username", _usernameInput.Text },
			{ "Health", 100 },
			{ "Coins", 30 },
		};
		MattohaSystem.Instance.Client.SetPlayerData(data);
	}

	public override void _ExitTree()
	{

		MattohaSystem.Instance.Client.SetPlayerDataSucceed -= OnSetPlayerData;
		base._ExitTree();
	}
}
