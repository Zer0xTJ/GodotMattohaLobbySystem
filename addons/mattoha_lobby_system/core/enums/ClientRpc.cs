namespace Mattoha.Core.Demo;

internal enum ClientRpc
{
	RegisterPlayer,

	SetPlayerData,
	SetPlayerDataFailed,

	CreateLobby,
	CreateLobbyFailed,

	LoadAvailableLobbies,
	LoadAvailableLobbiesFailed,

	JoinLobby,
	JoinLobbyFailed,

	StartGame,
	StartGameFailed,

	LoadLobbyPlayers,
	LoadLobbyPlayersFailed,

	SpawnNode,
	SpawnNodeFailed,

	SpawnLobbyNodes,
	SpawnLobbyNodesFailed,

	DespawnNode,
	DespawnNodeFailed,

	DespawnRemovedSceneNodes,
	DespawnRemovedSceneNodesFailed,
}
