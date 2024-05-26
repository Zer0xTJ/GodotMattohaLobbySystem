extends CharacterBody2D

@export var speed := 200.0

func _process(delta):
    if (multiplayer.get_unique_id() == get_multiplayer_authority()):
        var direction = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down").normalized()
        velocity = direction * speed
        move_and_slide()
