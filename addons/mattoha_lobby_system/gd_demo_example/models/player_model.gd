extends MattohaPlayer
class_name PlayerModel

func _to_string():
    return "Player(%d, %s)" % [Id, Username]