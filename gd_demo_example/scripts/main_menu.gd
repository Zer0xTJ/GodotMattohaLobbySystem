extends Control

func _ready():
	MattohaSystemGD.client.connected_to_server.connect(_on_connected_to_server)

func _on_connected_to_server():
	get_tree().change_scene_to_file("res://gd_demo_example/scenes/user_dialog.tscn")

func _on_client_button_pressed():
	MattohaSystemGD.start_client()

func _on_server_button_pressed():
	MattohaSystemGD.start_server()
	get_tree().change_scene_to_file("res://gd_demo_example/scenes/game_holder.tscn")
