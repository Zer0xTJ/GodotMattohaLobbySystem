extends HBoxContainer

var lobby_dict: Dictionary

func _ready():
	%LobbyNameLabel.text = lobby_dict["Name"]
	%PlayersCountLabel.text = "%d / %d" % [lobby_dict["PlayersCount"],lobby_dict["MaxPlayers"]]

func _on_join_button_pressed():
	MattohaSystem.Client.JoinLobby(lobby_dict["Id"])
