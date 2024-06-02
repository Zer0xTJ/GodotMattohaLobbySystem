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

	NewPlayerJoined,
	PlayerLeft,

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

	JoinedPlayerUpdated,

	SetLobbyData,
	SetLobbyDataFailed,

	SetLobbyOwner,
	SetLobbyOwnerFailed,

	JoinTeam,
	JoinTeamFailed,

	SendTeamMessage,
	SendTeamMessageFailed,

	SendLobbyMessage,
	SendLobbyMessageFailed,

	SendGlobalMessage,
	SendGlobalMessageFailed,
	PlayerChangedHisTeam,

	LeaveLobby,
}
