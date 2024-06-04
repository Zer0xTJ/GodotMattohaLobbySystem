extends Control

@export var usernameInput: LineEdit;

func _ready():
    MattohaSystem.Client.SetPlayerDataSucceed.connect(_on_set_player_data_succeed)

func _on_continue_button_pressed():
    MattohaSystem.Client.SetPlayerData({"Username": usernameInput.text})

func _on_set_player_data_succeed(_player_data: Dictionary):
    get_tree().change_scene_to_file("res://gd_demo_example/scenes/lobbies.tscn")