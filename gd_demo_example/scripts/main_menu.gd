extends Control

func _ready():
    MattohaSystem.Client.ConnectedToServer.connect(_on_connected_to_server)

func _on_connected_to_server():
    get_tree().change_scene("res://gd_demo_example/scenes/lobbies.tscn")

func _on_client_button_pressed():
    MattohaSystem.ConnectToServer()

func _on_server_button_pressed():
    MattohaSystem.StartServer()
