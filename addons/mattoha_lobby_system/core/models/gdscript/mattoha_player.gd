class_name MattohaPlayer

var Id: int
var JoinedLobbyId: int
var TeamId: int
var Username: String
var PrivateProps: Array[String] = []
var ChatProps: Array[String] = ["Id", "Username"]

func to_dict():
	return {
		"Id": Id,
		"JoinedLobbyId": JoinedLobbyId,
		"TeamId": TeamId,
		"Username": Username,
		"PrivateProps": PrivateProps,
		"ChatProps": ChatProps
	}
