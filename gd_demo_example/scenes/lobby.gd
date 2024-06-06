extends Control

@export var LobbyNameLabel: Label;
@export var PlayersContainer: VBoxContainer;
@export var MessagesContainer: VBoxContainer;
@export var MessageInput: LineEdit;

func _ready():
    MattohaSystemGD.client.start_game_succeed.connect(_on_start_game)
    MattohaSystemGD.client.load_lobby_players_succeed.connect(_on_load_lobby_players)
    MattohaSystemGD.client.new_player_joined.connect(_on_new_player_joined)
    MattohaSystemGD.client.set_lobby_data_succeed.connect(_on_set_lobby_data_succeed)
    MattohaSystemGD.client.player_changed_his_team.connect(_on_player_changed_his_team)
    MattohaSystemGD.client.team_message_received.connect(_on_team_message)
    MattohaSystemGD.client.lobby_message_received.connect(_on_lobby_message)
    MattohaSystemGD.client.global_message_received.connect(_on_global_message)
    MattohaSystemGD.client.player_left.connect(_on_player_left)

    MattohaSystemGD.client.load_lobby_players()
    refresh_lobby_name()

func _on_start_game(_lobby_data: Dictionary):
    get_tree().change_scene_to_file("res://gd_demo_example/scenes/game_holder.tscn")

func refresh_lobby_name():
    LobbyNameLabel.text = MattohaSystemGD.client.current_lobby["Name"]

func update_joined_players():
    var players = MattohaSystemGD.client.current_lobby_players.values()
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
    MattohaSystemGD.client.set_lobby_data({"Name": "LOBB%d" % (randi() % 1000)})

func _on_team_1_button_pressed():
    MattohaSystemGD.client.join_team(0)

func _on_team_2_button_pressed():
    MattohaSystemGD.client.join_team(1)

func _on_team_message_button_pressed():
    MattohaSystemGD.client.send_team_message(MessageInput.text)
    MessageInput.text = ""

func _on_lobby_message_button_pressed():
    MattohaSystemGD.client.send_lobby_message(MessageInput.text)
    MessageInput.text = ""

func _on_global_message_button_pressed():
    MattohaSystemGD.client.send_global_message(MessageInput.text)
    MessageInput.text = ""

func _on_start_game_button_pressed():
    MattohaSystemGD.client.start_game()

func _on_leave_lobby_button_pressed():
    MattohaSystemGD.client.leave_lobby()
    get_tree().change_scene_to_file("res://gd_demo_example/scenes/lobbies.tscn")
