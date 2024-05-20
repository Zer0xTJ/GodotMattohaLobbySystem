using Godot;
using MattohaLobbySystem.Demo;

public partial class Coin : Area2D
{

    public void OnBodyEntered(Node2D body)
    {
        MyLobbyManager.System!.Client!.DespawnNode(this);
        QueueFree();
    }
}
