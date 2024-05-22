extends Control

@export var lobby_name: LineEdit

func _ready():
    LobbyManager.system.Client.NewLobbyCreated.connect(_on_lobby_created)

func _on_lobby_created(lobby: Dictionary):
    print("lobby created: ", lobby)
    get_tree().change_scene_to_file("res://addons/mattoha_lobby_system/gd_demo_example/scenes/lobby.tscn")

func _on_create_button_pressed():
    var lobby = LobbyModel.new()
    lobby.Name = lobby_name.text
    lobby.MaxPlayers = 4
    LobbyManager.system.Client.CreateLobby(lobby.to_dict())