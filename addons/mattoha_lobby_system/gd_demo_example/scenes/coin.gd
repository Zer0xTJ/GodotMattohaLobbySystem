extends Area2D

func _on_body_entered(body: Node2D):
    LobbyManager.system.Client.DespawnNode(self)
    queue_free()
