extends Control

func _ready():
    LobbyManager.system.Client.ConnectedToServer.connect(_on_connected_to_server)

func _on_connected_to_server():
    print("Connected to server.")
    get_tree().change_scene_to_file("res://addons/mattoha_lobby_system/gd_demo_example/scenes/user_dialog.tscn")

func _on_server_button_pressed():
    LobbyManager.system.Server.StartServer()
    queue_free()

func _on_client_button_pressed():
    LobbyManager.system.Client.ConnectToServer()