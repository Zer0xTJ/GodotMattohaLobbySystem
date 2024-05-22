extends Control

@export var username_input: LineEdit

func _ready():
	LobbyManager.system.Client.CurrentPlayerUpdated.connect(_on_current_player_updated)

func _on_continue_button_pressed():
	var player = PlayerModel.new()
	player.Username = username_input.text
	LobbyManager.system.Client.SetPlayerData(player.to_dict())

func _on_current_player_updated(player):
	get_tree().change_scene_to_file("res://addons/mattoha_lobby_system/gd_demo_example/scenes/lobbies.tscn")
