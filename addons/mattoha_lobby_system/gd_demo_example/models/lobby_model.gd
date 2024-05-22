extends MattohaLobby
class_name LobbyModel

func _to_string():
    return "Lobby(%d, %s, owner=%d)" % [Id, Name, OwnerId]