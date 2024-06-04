extends Control

@export var LobbyNameLabel: Label;
@export var PlayersContainer: VBoxContainer;
@export var MessagesContainer: VBoxContainer;
@export var MessageInput: LineEdit;

func _ready():
    MattohaSystem.Client.StartGameSucceed.connect(_on_start_game)
    MattohaSystem.Client.LoadLobbyPlayersSucceed.connect(_on_load_lobby_players)
    MattohaSystem.Client.NewPlayerJoined.connect(_on_new_player_joined)
    MattohaSystem.Client.SetLobbyDataSucceed.connect(_on_set_lobby_data_succeed)
    MattohaSystem.Client.PlayerChangedHisTeam.connect(_on_player_changed_his_team)
    MattohaSystem.Client.TeamMessageRecieved.connect(_on_team_message)
    MattohaSystem.Client.LobbyMessageRecieved.connect(_on_lobby_message)
    MattohaSystem.Client.GlobalMessageRecieved.connect(_on_global_message)
    MattohaSystem.Client.PlayerLeft.connect(_on_player_left)

    MattohaSystem.Client.LoadLobbyPlayers()
    refresh_lobby_name()

func _on_start_game(_lobby_data: Dictionary):
    get_tree().change_scene_to_file("res://gd_demo_example/scenes/game_holder.tscn")

func refresh_lobby_name():
    LobbyNameLabel.text = MattohaSystem.Client.CurrentLobby["Name"]

func update_joined_players():
    var players = MattohaSystem.Client.CurrentLobbyPlayers.values()
    for slot in PlayersContainer.get_children():
        slot.queue_free()
    for player in players:
        var player_label = Label.new()
        player_label.text = "Player(%d, %s, %d)" % [player["Id"],player["Username"],player["TeamId"]]
        PlayersContainer.add_child(player_label)

func _on_load_lobby_players(_players: Array):
    update_joined_players()

func _on_new_player_joined(_player: Dictionary):
    update_joined_players()

func _on_set_lobby_data_succeed(_lobbyData: Dictionary):
    refresh_lobby_name()

func _on_player_changed_his_team(_player: Dictionary):
    update_joined_players()

func _on_team_message(_player: Dictionary, _message: String):
    var label = Label.new()
    label.text = "Team(%s, %s)" % [_player["Username"],_message]
    MessagesContainer.add_child(label)

func _on_lobby_message(_player: Dictionary, _message: String):
    var label = Label.new()
    label.text = "Lobby(%s, %s)" % [_player["Username"],_message]
    MessagesContainer.add_child(label)

func _on_global_message(_player: Dictionary, _message: String):
    var label = Label.new()
    label.text = "Global(%s)" % [_message]
    MessagesContainer.add_child(label)

func _on_player_left(_player: Dictionary):
    update_joined_players()

func _on_random_name_button_pressed():
    MattohaSystem.Client.SetLobbyData({"Name": "LOBB%d" % (randi() % 1000)})

func _on_team_1_button_pressed():
    MattohaSystem.Client.JoinTeam(0)

func _on_team_2_button_pressed():
    MattohaSystem.Client.JoinTeam(1)

func _on_team_message_button_pressed():
    MattohaSystem.Client.SendTeamMessage(MessageInput.text)
    MessageInput.text = ""

func _on_lobby_message_button_pressed():
    MattohaSystem.Client.SendLobbyMessage(MessageInput.text)
    MessageInput.text = ""

func _on_global_message_button_pressed():
    MattohaSystem.Client.SendGlobalMessage(MessageInput.text)
    MessageInput.text = ""

func _on_start_game_button_pressed():
    MattohaSystem.Client.StartGame()

func _on_leave_lobby_button_pressed():
    MattohaSystem.Client.LeaveLobby()
    get_tree().change_scene_to_file("res://gd_demo_example/scenes/lobbies.tscn")
