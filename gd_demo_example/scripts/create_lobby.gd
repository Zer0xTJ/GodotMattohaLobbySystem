extends Control

@export var lobbyNameInput: LineEdit;

func _ready():
	MattohaSystem.Client.CreateLobbySucceed.connect(_on_create_lobby_succeed)

func _on_create_lobby_succeed(_lobby_data: Dictionary):
	get_tree().change_scene_to_file("res://gd_demo_example/scenes/lobby.tscn")

func _on_create_button_pressed():
	var lobby: Dictionary = {
		"Name": lobbyNameInput.text,
		"MaxPlayers": 4,
	}
	MattohaSystem.Client.CreateLobby(lobby, "res://gd_demo_example/scenes/game.tscn")
