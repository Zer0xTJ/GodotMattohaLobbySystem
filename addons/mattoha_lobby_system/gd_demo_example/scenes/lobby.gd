extends Control

@export var LobbyName: Label

@export var PlayersContainer: VBoxContainer

@export var ChatInput: LineEdit
@export var MessagesContainer: VBoxContainer

func _ready():
	LobbyManager.system.Client.JoinedLobbyRefreshed.connect(_on_lobby_updated)
	LobbyManager.system.Client.JoinedPlayersRefreshed.connect(_on_joined_players_updated)

	LobbyManager.system.Client.GlobalMessageReceived.connect(_on_global_message)
	LobbyManager.system.Client.LobbyMessageReceived.connect(_on_lobby_message)
	LobbyManager.system.Client.TeamMessageReceived.connect(_on_team_message)

	refresh_lobby_name()
	LobbyManager.system.Client.RefreshJoinedPlayers()

func refresh_lobby_name():
	var lobbyDict = LobbyManager.system.Client.GetCurrentLobbyData()
	LobbyName.text = lobbyDict["Name"]

func _on_lobby_updated(lobby: Dictionary):
	refresh_lobby_name()

func _on_joined_players_updated(playersDict: Dictionary):
	var players = playersDict.values()
	for slot in PlayersContainer.get_children():
		slot.queue_free()
	for player in players:
		var label = Label.new()
		label.text = "Player(%d, %s, Team=%d)" % [player["Id"],player["Username"],player["TeamId"]]
		PlayersContainer.add_child(label)

func _on_global_message(message: String, player: Dictionary):
	var label = Label.new()
	label.text = "Global(%s): %s" % [player["Username"],message]
	MessagesContainer.add_child(label)

func _on_lobby_message(message: String, player: Dictionary):
	var label = Label.new()
	label.text = "Lobby(%s): %s" % [player["Username"],message]
	MessagesContainer.add_child(label)

func _on_team_message(message: String, player: Dictionary):
	var label = Label.new()
	label.text = "Team(%s): %s" % [player["Username"],message]
	MessagesContainer.add_child(label)

func _on_random_name_button_pressed():
	var current_lobby = LobbyManager.system.Client.GetCurrentLobbyData()
	current_lobby["Name"] = randi() % 1000
	LobbyManager.system.Client.SetLobbyData(current_lobby)

func _on_refresh_lobby_button_pressed():
	LobbyManager.system.Client.RefreshLobbyData()

func _on_team_1_button_pressed():
	LobbyManager.system.Client.SetTeam(0)

func _on_team_2_button_pressed():
	LobbyManager.system.Client.SetTeam(1)

func _on_refresh_players_button_pressed():
	LobbyManager.system.Client.RefreshJoinedPlayers()

func _on_team_message_button_pressed():
	LobbyManager.system.Client.SendTeamMessage(ChatInput.text)
	ChatInput.text = ""

func _on_lobby_message_button_pressed():
	LobbyManager.system.Client.SendLobbyMessage(ChatInput.text)
	ChatInput.text = ""

func _on_global_message_button_pressed():
	LobbyManager.system.Client.SendGlobalMessage(ChatInput.text)
	ChatInput.text = ""

func _on_leave_lobby_button_pressed():
	LobbyManager.system.Client.LeaveLobby()
	get_tree().change_scene_to_file("res://addons/mattoha_lobby_system/gd_demo_example/scenes/lobbies.tscn")

func _on_start_game_button_pressed():
	LobbyManager.system.Client.StartGame()
	get_tree().change_scene_to_file("res://addons/mattoha_lobby_system/gd_demo_example/scenes/game.tscn")
