extends Node
class_name MattohaClient

## Emmited when connection to server is succeed.
signal connected_to_server()

## Emmited when player is connected and registered in server memory.
## [br][param player_data] Initial player data.
signal player_registered(player_data: Dictionary)

## Emmited when loading lobby players loaded successfully.
## [br][param players] Joined players in lobby.
signal load_lobby_players_succeed(players: Array)

## Emmited when loading lobby player failed.
## [br][param cause] Fail cause.
signal load_lobby_players_failed(cause: String)

## Emmited when available lobbies loaded successfully.
## [br][param lobbies] Available lobbies.
signal load_available_lobbies_succeed(lobbies: Array)

## Emmited when available lobbies failed to load.
## [br][param cause] Fail cause.
signal load_available_lobbies_failed(cause: String)

## Emitted when setting player data succeeds.
## [br][param player_data] The updated player data.
signal set_player_data_succeed(player_data: Dictionary)

## Emitted when setting player data fails.
## [br][param cause] The cause of the failure.
signal set_player_data_failed(cause: String)

## Emitted when creating a lobby succeeds.
## [br][param lobby_data] The data of the created lobby.
signal create_lobby_succeed(lobby_data: Dictionary)

## Emitted when creating a lobby fails.
## [br][param cause] The cause of the failure.
signal create_lobby_failed(cause: String)

## Emitted when setting lobby data succeeds.
## [br][param lobby_data] The updated lobby data.
signal set_lobby_data_succeed(lobby_data: Dictionary)

## Emitted when setting lobby data fails.
## [br][param cause] The cause of the failure.
signal set_lobby_data_failed(cause: String)

## Emitted when setting the lobby owner succeeds.
## [br][param lobby_data] The updated lobby data with the new owner.
signal set_lobby_owner_succeed(lobby_data: Dictionary)

## Emitted when setting the lobby owner fails.
## [br][param cause] The cause of the failure.
signal set_lobby_owner_failed(cause: String)

## Emitted when starting the game succeeds.
## [br][param lobby_data] The updated lobby data after starting the game.
signal start_game_succeed(lobby_data: Dictionary)

## Emitted when starting the game fails.
## [br][param cause] The cause of the failure.
signal start_game_failed(cause: String)

## Emitted when ending the game succeeds.
## [br][param lobby_data] The updated lobby data after ending the game.
signal end_game_succeed(lobby_data: Dictionary)

## Emitted when ending the game fails.
## [br][param cause] The cause of the failure.
signal end_game_failed(cause: String)

## Emitted when joining a lobby succeeds.
## [br][param lobby_data] The data of the joined lobby.
signal join_lobby_succeed(lobby_data: Dictionary)

## Emitted when joining a lobby fails.
## [br][param cause] The cause of the failure.
signal join_lobby_failed(cause: String)

## Emitted when leaving a lobby succeeds.
signal leave_lobby_succeed()

## Emitted when joining a team succeeds.
## [br][param new_team] The team the player joined.
signal join_team_succeed(new_team: int)

## Emitted when joining a team fails.
## [br][param cause] The cause of the failure.
signal join_team_failed(cause: String)

## Emitted when a new player joins.
## [br][param player_data] The data of the new player.
signal new_player_joined(player_data: Dictionary)

## Emitted when a joined player updates their information.
## [br][param player_data] The updated player data.
signal joined_player_updated(player_data: Dictionary)

## Emitted when a player changes their team.
## [br][param player_data] The data of the player who changed teams.
signal player_changed_his_team(player_data: Dictionary)

## Emitted when a player joins current lobby.
## [br][param player_data] The data of the player who joined.
signal player_joined(player_data: Dictionary)

## Emitted when a player leaves.
## [br][param player_data] The data of the player who left.
signal player_left(player_data: Dictionary)

## Emitted when a node spawn is requested.
## [br][param node_data] The data of the node to be spawned.
signal spawn_node_requested(node_data: Dictionary)

## Emitted when node spawning fails.
## [br][param cause] The cause of the failure.
signal spawn_node_failed(cause: String)

## Emitted when spawning lobby nodes fails.
## [br][param cause] The cause of the failure.
signal spawn_lobby_nodes_failed(cause: String)

## Emitted when a node despawn is requested.
## [br][param node_data] The data of the node to be despawned.
signal despawn_node_requested(node_data: Dictionary)

## Emitted when node despawning fails.
## [br][param cause] The cause of the failure.
signal despawn_node_failed(cause: String)

## Emitted when despawning removed scene nodes fails.
## [br][param cause] The cause of the failure.
signal despawn_removed_scene_nodes_failed(cause: String)

## Emitted when a global message is received.
## [br][param sender_data] The data of the message sender.
## [br][param message] The received message.
signal global_message_received(sender_data: Dictionary, message: String)

## Emitted when sending a global message fails.
## [br][param cause] The cause of the failure.
signal global_message_failed(cause: String)

## Emitted when a lobby message is received.
## [br][param sender_data] The data of the message sender.
## [br][param message] The received message.
signal lobby_message_received(sender_data: Dictionary, message: String)

## Emitted when sending a lobby message fails.
## [br][param cause] The cause of the failure.
signal lobby_message_failed(cause: String)

## Emitted when a team message is received.
## [br][param sender_data] The data of the message sender.
## [br][param message] The received message.
signal team_message_received(sender_data: Dictionary, message: String)

## Emitted when sending a team message fails.
## [br][param cause] The cause of the failure.
signal team_message_failed(cause: String)

## The GameHolder node in game tree.
var game_holder: Node: get = _get_game_holder, set = _set_game_holder
## Returns The LobbyNode (Current arena game scene of lobby) in game tree.
var lobby_node: Node: get = _get_lobby_node, set = _set_lobby_node
## Returns the current player data synced with all players, "PrivateProps" list is not synced with others.
var current_player: Dictionary: get = _get_current_player, set = _set_current_player
## Returns the current player data synced with all players in lobby, "PrivateProps" list is not synced with others.
var current_lobby: Dictionary: get = _get_current_lobby, set = _set_current_lobby
## Returns the dictionary containing joined players in current lobby, without PrivateProps items.
var current_lobby_players: Dictionary: get = _get_current_lobby_players, set = _set_current_lobby_players
## Returns true if player "IsInGame" proeprty is true, IsInGame will be true when player sync spawned lobby nodes by others.
var can_replicate: bool: get = _get_can_replicate, set = _set_can_replicate

func _ready():
    MattohaSystem.Client.ConnectedToServer.connect(_on_connected_to_server)
    MattohaSystem.Client.PlayerRegistered.connect(_on_player_registered)
    MattohaSystem.Client.LoadLobbyPlayersSucceed.connect(_on_load_lobby_players_succeed)
    MattohaSystem.Client.LoadLobbyPlayersFailed.connect(_on_load_lobby_players_failed)
    MattohaSystem.Client.LoadAvailableLobbiesSucceed.connect(_on_load_available_lobbies_succeed)
    MattohaSystem.Client.LoadAvailableLobbiesFailed.connect(_on_load_available_lobbies_failed)
    MattohaSystem.Client.SetPlayerDataSucceed.connect(_on_set_player_data_succeed)
    MattohaSystem.Client.SetPlayerDataFailed.connect(_on_set_player_data_failed)
    MattohaSystem.Client.CreateLobbySucceed.connect(_on_create_lobby_succeed)
    MattohaSystem.Client.CreateLobbyFailed.connect(_on_create_lobby_failed)
    MattohaSystem.Client.SetLobbyDataSucceed.connect(_on_set_lobby_data_succeed)
    MattohaSystem.Client.SetLobbyDataFailed.connect(_on_set_lobby_data_failed)
    MattohaSystem.Client.SetLobbyOwnerSucceed.connect(_on_set_lobby_owner_succeed)
    MattohaSystem.Client.SetLobbyOwnerFailed.connect(_on_set_lobby_owner_failed)
    MattohaSystem.Client.StartGameSucceed.connect(_on_start_game_succeed)
    MattohaSystem.Client.StartGameFailed.connect(_on_start_game_failed)
    MattohaSystem.Client.EndGameSucceed.connect(_on_end_game_succeed)
    MattohaSystem.Client.EndGameFailed.connect(_on_end_game_failed)
    MattohaSystem.Client.JoinLobbySucceed.connect(_on_join_lobby_succeed)
    MattohaSystem.Client.JoinLobbyFailed.connect(_on_join_lobby_failed)
    MattohaSystem.Client.LeaveLobbySucceed.connect(_on_leave_lobby_succeed)
    MattohaSystem.Client.JoinTeamSucceed.connect(_on_join_team_succeed)
    MattohaSystem.Client.JoinTeamFailed.connect(_on_join_team_failed)
    MattohaSystem.Client.NewPlayerJoined.connect(_on_new_player_joined)
    MattohaSystem.Client.JoinedPlayerUpdated.connect(_on_joined_player_updated)
    MattohaSystem.Client.PlayerChangedHisTeam.connect(_on_player_changed_his_team)
    MattohaSystem.Client.PlayerJoined.connect(_on_player_joined)
    MattohaSystem.Client.PlayerLeft.connect(_on_player_left)
    MattohaSystem.Client.SpawnNodeRequested.connect(_on_spawn_node_requested)
    MattohaSystem.Client.SpawnNodeFailed.connect(_on_spawn_node_failed)
    MattohaSystem.Client.SpawnLobbyNodesFailed.connect(_on_spawn_lobby_nodes_failed)
    MattohaSystem.Client.DespawnNodeRequested.connect(_on_despawn_node_requested)
    MattohaSystem.Client.DespawnNodeFailed.connect(_on_despawn_node_failed)
    MattohaSystem.Client.DespawnRemovedSceneNodesFailed.connect(_on_despawn_removed_scene_nodes_failed)
    MattohaSystem.Client.GlobalMessageRecieved.connect(_on_global_message_received)
    MattohaSystem.Client.GlobalMessageFailed.connect(_on_global_message_failed)
    MattohaSystem.Client.LobbyMessageRecieved.connect(_on_lobby_message_received)
    MattohaSystem.Client.LobbyMessageFailed.connect(_on_lobby_message_failed)
    MattohaSystem.Client.TeamMessageRecieved.connect(_on_team_message_received)
    MattohaSystem.Client.TeamMessageFailed.connect(_on_team_message_failed)
########################################################################################

## Returns joined lobby players Ids.
func get_lobby_players_ids() -> Array[int]:
    return MattohaSystem.Client.GetLobbyPlayersIds()

## Check if peer id is joined in lobby or not.
## [br][param player_id] The player id to check.
## [br][return] true if joined, otherwise false.
func is_player_in_lobby(player_id: int) -> bool:
    return MattohaSystem.Client.IsPlayerInLobby(player_id)

## Check if the player id is in same team.
## [br][param player_id] The player id to check.
## [br][return] true if in same team, otherwise false.
func is_player_in_my_team(player_id: int) -> bool:
    return MattohaSystem.Client.IsPlayerInMyTeam(player_id)

## Returns true if player entered the game scene and spawned lobby nodes, this will be used under the hood for replication purposes.
## [param player_id] The player id to check.
## [return] true if entered the game, otherwise false.
func is_player_in_game(player_id: int) -> bool:
    return MattohaSystem.Client.IsPlayerInGame(player_id)

## Set Player data in server, this will emit "SetPlayerDataSucceed" or "SetPlayerDataFailed", for current client
## and emmit "JoinedPlayerUpdated" for other lobby players.
## [br][br]
## Base player keys you can edit in addition to your custom keys:[br]
##  - Username {string}[br]
##  - TeamId {int}[br]
##  - PrivateProps {Godot Array of string}[br]
##  - ChatProps {Godot Array of string}[br]
## [br][param player_data] A dictionary containing the keys to update, its ok to put only one key, no need for full player data.
func set_player_data(player_data: Dictionary):
    MattohaSystem.Client.SetPlayerData(player_data)

## Create new lobby on server, this will emmit "CreateLobbySucceed and SetPlayerDataSucceed" or "CreateLobbyFailed" for creator,
## and "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled.
## [br][br]
## Base lobby keys you can edit in addition to your custom keys:[br]
##  - Name {string}[br]
##  - MaxPlayers {int}[br]
##  - PrivateProps {Godot Array of string}[br]
## [br][param lobby_data] Lobby Data to create.
## [br][param lobby_scene_file] The scene file for lobby game, for example: "res://maps/arean.tscn".
func create_lobby(lobby_data: Dictionary, lobby_scene_file: String):
    MattohaSystem.Client.CreateLobby(lobby_data, lobby_scene_file)

## Set Lobby data and sync it, this will emmit "SetLobbyDataFailed" for caller user if Setting Data failed,
## and sync the data for all joined players - including caller user - then emmit "SetLobbyDataSucceed" for them,
## and "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled.
## Base player keys you can edit in addition to your custom keys:[br]
##  - Username {string}[br]
##  - TeamId {int}[br]
##  - PrivateProps {Godot Array of string}[br]
##  - LobbySceneFile {string}
## [br][param player_data] A dictionary containing the keys to update, its ok to put only one key, no need for full player data.
func set_lobby_data(lobby_data: Dictionary):
    MattohaSystem.Client.SetLobbyData(lobby_data)

## Change lobby owner, this will emmit "SetLobbyOwnerSucceed" for all joined players including caller user if changing owner is succeed,
## and emmit "SetLobbyOwnerFailed" for caller user if setting owner fails, ONLY owner of the lobby can set new lobby owner.
func set_lobby_owner(new_owner_id: int):
    MattohaSystem.Client.SetLobbyOwner(new_owner_id)

## Load Available lobbies, this will emmit "LoadAvailableLobbiesSucceed" or "LoadAvailableLobbiesFailed" for caller user.
func load_available_lobbies():
    MattohaSystem.Client.LoadAvailableLobbies()

## Join lobby by it's ID, this will emmit "JoinLobbySucceed" or "JoinLobbyFailed" for caller user,
## and "NewPlayerJoined" for other joined players,
## and "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled.
## [br][param lobby_id] Loby id to join to.
func join_lobby(lobby_id: int):
    MattohaSystem.Client.JoinLobby(lobby_id)

## Start Lobby Game, this will emmit "StartGameSucceed" for all joined players or "StartGameFailed" for caller user,
## and "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled.
func start_game():
    MattohaSystem.Client.StartGame()

## End Lobby Game, this ewill emmit "EndGameSucceed" for all joined players or "EndGameFailed" for caller user,
## and "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled.
func end_game():
    MattohaSystem.Client.EndGame()

## Load players data that joined in same lobby, this will emmit "LoadLobbyPlayersSucceed" or "LoadLobbyPlayersFailed"
## for the caller user.
func load_lobby_players():
    MattohaSystem.Client.LoadLobbyPlayers()

## Spawn node for all joined players in same lobby or same team, this will emmit "SpawnNodeRequested" for joined players or players in teams,
## "SpawnNodeFailed" will be emmited and node will be locally despawned if spawning node failed.
## [br][param node] Node object (should be existing in tree).
## [br][param team_only] true if spawning should be for team only.
## [br][param additional_props] Additional properties that will be synced during spawning node across players.
func spawn_node(node: Node, team_only: bool=false, additional_props: Array[String]=[]):
    MattohaSystem.Client.SpawnNode(node, team_only, additional_props)

## Despawn node and sync that with all players, this will emmit "DespawnNodeRequested" for all players and "DespawnNodeFailed" for caller user
## if despawning fails and then respawning the node in case if player destroyed it.
## [br][param node] Node object (should be existing in tree).
func despawn_node(node: Node):
    MattohaSystem.Client.DespawnNode(node)

## Spawn all lobby nodes tha has been spawned by other players, team only nodes will not be spawned until player in same team,
## this method will emmit "SpawnLobbyNodesSucceed" or  "SpawnLobbyNodesFailed".
func spawn_lobby_nodes():
    MattohaSystem.Client.SpawnLobbyNodes()

## Despawn All scene nodes that despawned during game player, used by gameholder when new player enter the game scene,
## this will emmit "DespawnRemovedSceneNodesSucceed" or "DespawnRemovedSceneNodesFailed".
func despawn_removed_scene_nodes():
    MattohaSystem.Client.DespawnRemovedSceneNodes()

## Join or change a team, this will emmit "JoinTeamSucceed" or "JoinTeamFailed" if joingin failed,
## in addition, "PlayerChangedHisTeam" will be emmited on all lobby players including the player itself.
## [br][param team_id] Team ID to join to.
func join_team(team_id: int):
    MattohaSystem.Client.JoinTeam(team_id)

## Send a message for team members, this will emmit "TeamMessageSucceed" for all players or "TeamMessageFailed" for caller user.
## [br][param message] Message to send.
func send_team_message(message: String):
    MattohaSystem.Client.SendTeamMessage(message)

## Send a message for lobby members, this will emmit "LobbyMessageSucceed" for all players or "LobbyMessageFailed" for caller user.
## [br][param message] Message to send.
func send_lobby_message(message: String):
    MattohaSystem.Client.SendLobbyMessage(message)

## Send a global message for all online users, this will emmit "GlobalMessageSucceed" for all players or "GlobalMessageFailed" for caller user.
## [br][param message] Message to send.
func send_global_message(message: String):
    MattohaSystem.Client.SendGlobalMessage(message)

## Leave joined lobby, this will emmit "LeaveLobbySucceed" for caller user and "PlayerLeft" for all joined players.
func leave_lobby():
    MattohaSystem.Client.LeaveLobby()

########################################################################################
func _on_connected_to_server():
    emit_signal("connected_to_server")

func _on_player_registered(player_data: Dictionary):
    emit_signal("player_registered", player_data)

func _on_load_lobby_players_succeed(players: Array):
    emit_signal("load_lobby_players_succeed", players)

func _on_load_lobby_players_failed(cause: String):
    emit_signal("load_lobby_players_failed", cause)

func _on_load_available_lobbies_succeed(lobbies: Array):
    emit_signal("load_available_lobbies_succeed", lobbies)

func _on_load_available_lobbies_failed(cause: String):
    emit_signal("load_available_lobbies_failed", cause)

func _on_set_player_data_succeed(player_data: Dictionary):
    emit_signal("set_player_data_succeed", player_data)

func _on_set_player_data_failed(cause: String):
    emit_signal("set_player_data_failed", cause)

func _on_create_lobby_succeed(lobby_data: Dictionary):
    emit_signal("create_lobby_succeed", lobby_data)

func _on_create_lobby_failed(cause: String):
    emit_signal("create_lobby_failed", cause)

func _on_set_lobby_data_succeed(lobby_data: Dictionary):
    emit_signal("set_lobby_data_succeed", lobby_data)

func _on_set_lobby_data_failed(cause: String):
    emit_signal("set_lobby_data_failed", cause)

func _on_set_lobby_owner_succeed(lobby_data: Dictionary):
    emit_signal("set_lobby_owner_succeed", lobby_data)

func _on_set_lobby_owner_failed(cause: String):
    emit_signal("set_lobby_owner_failed", cause)

func _on_start_game_succeed(lobby_data: Dictionary):
    emit_signal("start_game_succeed", lobby_data)

func _on_start_game_failed(cause: String):
    emit_signal("start_game_failed", cause)

func _on_end_game_succeed(lobby_data: Dictionary):
    emit_signal("end_game_succeed", lobby_data)

func _on_end_game_failed(cause: String):
    emit_signal("end_game_failed", cause)

func _on_join_lobby_succeed(lobby_data: Dictionary):
    emit_signal("join_lobby_succeed", lobby_data)

func _on_join_lobby_failed(cause: String):
    emit_signal("join_lobby_failed", cause)

func _on_leave_lobby_succeed():
    emit_signal("leave_lobby_succeed")

func _on_join_team_succeed(new_team: int):
    emit_signal("join_team_succeed", new_team)

func _on_join_team_failed(cause: String):
    emit_signal("join_team_failed", cause)

func _on_new_player_joined(player_data: Dictionary):
    emit_signal("new_player_joined", player_data)

func _on_joined_player_updated(player_data: Dictionary):
    emit_signal("joined_player_updated", player_data)

func _on_player_changed_his_team(player_data: Dictionary):
    emit_signal("player_changed_his_team", player_data)

func _on_player_joined(player_data: Dictionary):
    emit_signal("player_joined", player_data)

func _on_player_left(player_data: Dictionary):
    emit_signal("player_left", player_data)

func _on_spawn_node_requested(node_data: Dictionary):
    emit_signal("spawn_node_requested", node_data)

func _on_spawn_node_failed(cause: String):
    emit_signal("spawn_node_failed", cause)

func _on_spawn_lobby_nodes_failed(cause: String):
    emit_signal("spawn_lobby_nodes_failed", cause)

func _on_despawn_node_requested(node_data: Dictionary):
    emit_signal("despawn_node_requested", node_data)

func _on_despawn_node_failed(cause: String):
    emit_signal("despawn_node_failed", cause)

func _on_despawn_removed_scene_nodes_failed(cause: String):
    emit_signal("despawn_removed_scene_nodes_failed", cause)

func _on_global_message_received(sender_data: Dictionary, message: String):
    emit_signal("global_message_received", sender_data, message)

func _on_global_message_failed(cause: String):
    emit_signal("global_message_failed", cause)

func _on_lobby_message_received(sender_data: Dictionary, message: String):
    emit_signal("lobby_message_received", sender_data, message)

func _on_lobby_message_failed(cause: String):
    emit_signal("lobby_message_failed", cause)

func _on_team_message_received(sender_data: Dictionary, message: String):
    emit_signal("team_message_received", sender_data, message)

func _on_team_message_failed(cause: String):
    emit_signal("team_message_failed", cause)

func _get_game_holder() -> Node:
    return MattohaSystem.Client.GameHolder
func _set_game_holder(_val):
    pass

func _get_lobby_node() -> Node:
    return MattohaSystem.Client.LobbyNode
func _set_lobby_node(_val):
    pass

func _get_current_player() -> Dictionary:
    return MattohaSystem.Client.CurrentPlayer
func _set_current_player(_val):
    pass

func _get_current_lobby() -> Dictionary:
    return MattohaSystem.Client.CurrentLobby
func _set_current_lobby(_val):
    pass

func _get_current_lobby_players() -> Dictionary:
    return MattohaSystem.Client.CurrentLobbyPlayers
func _set_current_lobby_players(_val):
    pass

func _get_can_replicate() -> bool:
    return MattohaSystem.Client.CanReplicate
func _set_can_replicate(_val):
    pass