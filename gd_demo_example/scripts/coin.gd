extends Area2D

func _on_body_entered(_body: Node2D):
    if (multiplayer.is_server()):
        queue_free()