extends Node2D

@export var PlayerScene: PackedScene

func _ready():
	if (multiplayer.is_server()):
		return
	spawn_player()

func spawn_player():
	var instance = MattohaSystem.CreateInstance("res://gd_demo_example/scenes/player.tscn")
	instance.rotation = 2.3
	instance.position = Vector2(randi() % 200, randi() % 100)
	add_child(instance)
