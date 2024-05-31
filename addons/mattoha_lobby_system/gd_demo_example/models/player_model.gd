extends MattohaPlayer
class_name PlayerModel

func _to_string():
    return "Player(%d, %s, lobby=%d, team=%d)" % [Id, Username, JoinedLobbyId, TeamId]
