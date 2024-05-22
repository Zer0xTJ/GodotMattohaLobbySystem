class_name MattohaLobby

var Id: int
var OwnerId: int
var PlayersCount: int
var MaxPlayers: int
var Name: String
var IsGameStarted: bool
var PrivateProps: Array[String] = []

func to_dict():
    return {
        "Id": Id,
        "OwnerId": OwnerId,
        "Name": Name,
        "MaxPlayers": MaxPlayers,
        "PlayersCount": PlayersCount,
        "IsGameStarted": IsGameStarted,
        "PrivateProps": PrivateProps
    }
