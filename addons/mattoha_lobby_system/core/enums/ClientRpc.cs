namespace Mattoha.Core.Demo;

internal enum ClientRpc
{
	RegisterPlayer,

	SetPlayerData,
	SetPlayerDataFailed,

	CreateLobby,
	CreateLobbyFailed,

	LoadAvailableLobbies,

	JoinLobby,
	JoinLobbyFailed,

	StartGame,
	StartGameFailed,

	LoadLobbyPlayers,

	SpawnNode,
	SpawnNodeFailed,

	DespawnNode,
	DespawnNodeFailed,
	SpawnLobbyNodes,
}
