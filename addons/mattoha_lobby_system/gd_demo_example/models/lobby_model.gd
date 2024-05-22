extends MattohaLobby
class_name LobbyModel

func _to_string():
    return "Lobby(%d, %s, owner=%d)" % [Id, Name, OwnerId]

func load_from_dict(dict: Dictionary):
    Id = dict.get("Id")
    Name = dict.get("Name")
    OwnerId = dict.get("OwnerId")
    PlayersCount = dict.get("PlayersCount")
    MaxPlayers = dict.get("MaxPlayers")