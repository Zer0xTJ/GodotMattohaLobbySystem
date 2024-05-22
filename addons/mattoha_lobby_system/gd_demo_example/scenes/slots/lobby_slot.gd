extends HBoxContainer
class_name LobbySlot

@export var LobbyName: Label
@export var Players: Label

var lobby: LobbyModel

func _ready():
    LobbyManager.system.Client.JoinLobbySucceed.connect(_on_lobby_joined)

    LobbyName.text = lobby.Name
    Players.text = "%d/%d" % [lobby.PlayersCount, lobby.MaxPlayers]

func _on_join_button_pressed():
    LobbyManager.system.Client.JoinLobby(lobby.Id)

func _on_lobby_joined(lobby: Dictionary):
    print("Joined lobby", lobby)
    get_tree().change_scene_to_file("res://addons/mattoha_lobby_system/gd_demo_example/scenes/lobby.tscn")