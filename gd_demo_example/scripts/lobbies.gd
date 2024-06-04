extends Control

@export var lobbyList: VBoxContainer;

func _ready():
    MattohaSystem.Client.LoadAvailableLobbiesSucceed.connect(_on_load_available_lobbies_succeed)
    MattohaSystem.Client.LoadAvailableLobbies()

func _on_create_lobby_button_pressed():
    get_tree().change_scene_to_file("res://gd_demo_example/scenes/create_lobby.tscn")

func _on_load_available_lobbies_succeed(lobbies):
    print("Lobbies: ", lobbies)
