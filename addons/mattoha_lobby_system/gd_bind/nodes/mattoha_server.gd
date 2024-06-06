extends Node
class_name MattohaServer

## Emmited when a player join a lobby.
## [br][param player_id] player id that joined.
## [br][param lobby_id] lobby id that player joined to.
signal player_joined_lobby(player_id: int, lobby_id: int)

## Emmited when a player leave a lobby.
## [br][param player_id] player id that left.
## [br][param lobby_id] lobby id that player left from.
signal player_left_lobby(player_id: int, lobby_id: int)

## Registered players dictionary, where the key is the id of player.
var players: Dictionary: get = _get_players, set = _set_players
## Created lobbies by users, a dictionary where the key is the id of lobby.
var lobbies: Dictionary: get = _get_lobbies, set = _set_lobbies
## A dictionary that contains spawned nodes for each lobby, where the key is the lobby id, and the value is an array of spawned nodes.
var spawned_nodes: Dictionary: get = _get_spawned_nodes, set = _set_spawned_nodes
## A dictionary that contains removed scene nodes that has been despawned during game play for each lobby,
## where the key is the lobby id, and the value is an array of removed scene nodes.
var removed_scene_nodes: Dictionary: get = _get_remvoved_scene_nodes, set = _set_removed_scene_nodes
## The game holder node that contains lobbies nodes and game plays.
var game_holder: Node: get = _get_game_holder, set = _set_game_holder

func _ready():
    MattohaSystem.Server.PlayerJoinedLobby.connect(_on_player_joined_lobby)
    MattohaSystem.Server.PlayerLeftLobby.connect(_on_player_left_lobby)

func _on_player_joined_lobby(player_id: int, lobby_id: int):
    emit_signal("player_joined_lobby", player_id, lobby_id)

func _on_player_left_lobby(player_id: int, lobby_id: int):
    emit_signal("player_left_lobby", player_id, lobby_id)
########################################################################################

## Given a spawn payload, find the node in spawned nodes.
## [br][param payload] Generated payload from "GenerateNodePayloadData" method.
## [br][return] null if not found.
func find_spawned_node(payload: Dictionary) -> Dictionary:
    return MattohaSystem.Server.FindSpawnedNode(payload)

## Given a despawn payload, find the node in removed scene nodes.
## [br][param payload] Generated payload from "GenerateNodePayloadData" method.
## [br][return] null if not found.
func find_removed_scene_node(payload: Dictionary) -> Dictionary:
    return MattohaSystem.Server.FindRemovedSceneNode(payload)

## Get player data by player id.
## [br][param player_id] player id to get data from.
## [br][return] player data, null if not found.
func get_player(player_id: int) -> Dictionary:
    return MattohaSystem.Server.GetPlayer(player_id)

## Get lobby that player is joined to by player ID.
## [br][param player_id] player id to get lobby from.
## [br][return] lobby data, null if not found.
func get_player_lobby(player_id: int) -> Dictionary:
    return MattohaSystem.Server.GetPlayerLobby(player_id)

## Get list of players that joined to a given lobby.
## [br][param lobby_id] lobby id to get players from.
## [br][return] Empty array if no players in lobby.
func get_lobby_players(lobby_id: int) -> Array:
    return MattohaSystem.Server.GetLobbyPlayers(lobby_id)

## Get list of players that joined to a given lobby but secure the dictionaries, meanining that
## any propert inside "PrivateProps" will be hidden.
## [br][param lobby_id] lobby id to get players from.
## [br][return] Empty array if no players in lobby.
func get_lobby_players_secured(lobby_id: int) -> Array:
    return MattohaSystem.Server.GetLobbyPlayersSecured(lobby_id)

## Send an RPC from server to all players in a given lobby.
## [br][param lobby_id] lobby id to send rpc to.
## [br][param method_name] RPC method name.
## [br][param payload] payload to send "Dictionary"
## [br][param secure_payload] if true, "MattohaUtils.ToSecuredDict()" will be applied on payload.
## [br][param super_peer] this peer id will recieve the original payload without secure it.
## [br][param ignore_peer] ignore this peer and dont send RPC for him, usefull if you want to ignore the sender.
func send_rpc_for_players_in_lobby(lobby_id: int, method_name: String, payload: Dictionary, secure_payload: bool=false, super_peer: int=0, ignore_peer: int=0):
    MattohaSystem.Server.SendRpcForPlayersInLobby(lobby_id, method_name, payload, secure_payload, super_peer, ignore_peer)

## Refresh all available lobbies for all peers if "AutoLoadAvailableLobbies" is enabled or "force" argument is true.
## [br][param force] if true, then ignore the value of "AutoLoadAvailablelobbies" and force
##  to load available lobbies for all connected players.
func refresh_available_lobbies_for_all(force: bool=false):
    MattohaSystem.Server.RefreshAvailableLobbiesForAll(force)

## Refresh joined players list for all joined players in a given lobbyId.
## [br][param lobby_id] lobby id to refresh players list for.
func load_lobby_players_for_all(lobby_id: int):
    MattohaSystem.Server.LoadLobbyPlayersForAll(lobby_id)

## Spawn node from server for all joined players in same lobby.
## [br][param payload] Generated payload from "GenerateNodePayloadData" method.
## [br][param lobbyId] lobby id to spawn node for.
func spawn_node(payload: Dictionary, lobbyId: int):
    MattohaSystem.Server.SpawnNode(payload, lobbyId)

## Despawn node from server by passing a payload generated by method "GenerateNodePayloadData()".
## [br][param payload] Generated payload from "GenerateNodePayloadData" method.
func despawn_node(payload: Dictionary):
    MattohaSystem.Server.DespawnNode(payload)

## Remove player from joiend lobby, this will emmit "LeaveLobby" for player removed, "SetPlayerData" for player removed, "PlayerLeft" and
## "LoadLobbyPlayersSucceed" for joined players, "LoadAvailableLobbiesSucceed" for all online players if "AutoLoadAvailableLobbies" is enabled,
## in addition to sync data for players.
## [br][param player_id] player id to remove.
func remove_player_from_lobby(player_id: int):
    MattohaSystem.Server.RemovePlayerFromLobby(player_id)

########################################################################################
func _get_players() -> Dictionary:
    return MattohaSystem.Server.Players

func _set_players(_val):
    pass

func _get_lobbies() -> Dictionary:
    return MattohaSystem.Server.Lobbies

func _set_lobbies(_val):
    pass

func _get_spawned_nodes() -> Dictionary:
    return MattohaSystem.Server.SpawnedNodes

func _set_spawned_nodes(_val):
    pass

func _get_remvoved_scene_nodes() -> Dictionary:
    return MattohaSystem.Server.RemovedSceneNodes

func _set_removed_scene_nodes(_val):
    pass

func _get_game_holder() -> Node:
    return MattohaSystem.Server.GameHolder

func _set_game_holder(_val):
    pass