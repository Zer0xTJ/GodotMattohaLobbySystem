extends Control

func _ready():
	MattohaSystem.Client.ConnectedToServer.connect(_on_connected_to_server)

func _on_connected_to_server():
	get_tree().change_scene_to_file("res://gd_demo_example/scenes/user_dialog.tscn")

func _on_client_button_pressed():
	MattohaSystem.StartClient()

func _on_server_button_pressed():
	MattohaSystem.StartServer()
	get_tree().change_scene_to_file("res://gd_demo_example/scenes/game_holder.tscn")
