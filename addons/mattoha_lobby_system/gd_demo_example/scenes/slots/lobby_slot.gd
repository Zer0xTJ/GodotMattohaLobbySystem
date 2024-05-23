extends HBoxContainer
class_name LobbySlot

@export var LobbyName: Label
@export var Players: Label

var lobby: LobbyModel

func _ready():

    LobbyName.text = lobby.Name
    Players.text = "%d/%d" % [lobby.PlayersCount, lobby.MaxPlayers]

func _on_join_button_pressed():
    LobbyManager.system.Client.JoinLobby(lobby.Id)