extends Control

@export var LobbiesContainer: VBoxContainer
@export var LobbyScene: PackedScene

func _ready():
	LobbyManager.system.Client.JoinLobbySucceed.connect(_on_lobby_joined)
	LobbyManager.system.Client.AvailableLobbiesRefreshed.connect(_on_lobbies_loaded)
	LobbyManager.system.Client.LoadAvailableLobbies()

func _on_lobbies_loaded(lobbies: Array):
	for solt in LobbiesContainer.get_children():
		solt.queue_free()
	for lobby in lobbies:
		var lobbySlot = LobbyScene.instantiate() as LobbySlot
		var model = LobbyModel.new()
		model.load_from_dict(lobby)
		lobbySlot.lobby = model
		LobbiesContainer.add_child(lobbySlot)

func _on_create_lobby_button_pressed():
	get_tree().change_scene_to_file("res://addons/mattoha_lobby_system/gd_demo_example/scenes/create_lobby.tscn")

func _on_lobby_joined(lobby: Dictionary):
	print("Joined lobby", lobby)
	if (lobby["IsGameStarted"]):
		get_tree().change_scene_to_file("res://addons/mattoha_lobby_system/gd_demo_example/scenes/game.tscn")
	else:
		get_tree().change_scene_to_file("res://addons/mattoha_lobby_system/gd_demo_example/scenes/lobby.tscn")