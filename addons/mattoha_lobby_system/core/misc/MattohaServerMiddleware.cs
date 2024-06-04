using Godot;
using Godot.Collections;

namespace Mattoha.Misc;

public partial class MattohaServerMiddleware : Node
{

	/// <summary>
	/// Executed before setting player data
	/// </summary>
	/// <param name="payload">payload that contains keys and values</param>
	/// <param name="sender">the sender id, who want to change his data.</param>
	/// <returns>
	/// dictionary contains "Status" and "Message", when status false, execution will be terminated
	/// and error event with "Message" will be emmited to client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeSetPlayerData(Dictionary<string, Variant> payload, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after setting player data.
	/// </summary>
	/// <param name="payload">the final payload.</param>
	/// <param name="sender">sender id</param>
	public virtual void AfterSetPlayerData(Dictionary<string, Variant> payload, long sender) { }

	/// <summary>
	/// Executed before creating a lobby.
	/// </summary>
	/// <param name="lobbyData">The data for the lobby to be created.</param>
	/// <param name="sender">The ID of the sender creating the lobby.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeCreateLobby(Dictionary<string, Variant> lobbyData, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after creating a lobby.
	/// </summary>
	/// <param name="lobbyData">The data of the created lobby.</param>
	/// <param name="sender">The ID of the sender who created the lobby.</param>
	public virtual void AfterCreateLobby(Dictionary<string, Variant> lobbyData, long sender) { }

	/// <summary>
	/// Executed before setting lobby data.
	/// </summary>
	/// <param name="lobbyId">The ID of the lobby.</param>
	/// <param name="payload">The payload containing keys and values to be set.</param>
	/// <param name="sender">The ID of the sender setting the data.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeSetLobbyData(int lobbyId, Dictionary<string, Variant> payload, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after setting lobby data.
	/// </summary>
	/// <param name="lobbyId">The ID of the lobby.</param>
	/// <param name="payload">The payload containing the updated data.</param>
	/// <param name="sender">The ID of the sender who set the data.</param>
	public virtual void AfterSetLobbyData(int lobbyId, Dictionary<string, Variant> payload, long sender) { }

	/// <summary>
	/// Executed before setting the lobby owner.
	/// </summary>
	/// <param name="lobby">The data of the lobby where the owner is being set.</param>
	/// <param name="sender">The ID of the sender setting the owner.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeSetLobbyOwner(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after setting the lobby owner.
	/// </summary>
	/// <param name="lobby">The data of the lobby with the new owner set.</param>
	/// <param name="sender">The ID of the sender who set the owner.</param>
	public virtual void AfterSetLobbyOwner(Dictionary<string, Variant> lobby, long sender) { }

	/// <summary>
	/// Executed before loading available lobbies.
	/// </summary>
	/// <param name="sender">The ID of the sender requesting available lobbies.</param>
	/// <param name="lobbies">lobbies array.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeLoadAvailableLobbies(long sender, Array<Dictionary<string, Variant>> lobbies)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after loading available lobbies.
	/// </summary>
	/// <param name="sender">The ID of the sender who requested available lobbies.</param>
	/// <param name="lobbies">lobbies array.</param>
	public virtual void AfterLoadAvailableLobbies(long sender, Array<Dictionary<string, Variant>> lobbies) { }

	/// <summary>
	/// Executed before joining a lobby.
	/// </summary>
	/// <param name="lobby">The data of the lobby to join.</param>
	/// <param name="sender">The ID of the sender joining the lobby.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeJoinLobby(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after joining a lobby.
	/// </summary>
	/// <param name="lobby">The data of the joined lobby.</param>
	/// <param name="sender">The ID of the sender who joined the lobby.</param>
	public virtual void AfterJoinLobby(Dictionary<string, Variant> lobby, long sender) { }


	/// <summary>
	/// Executed before starting a game.
	/// </summary>
	/// <param name="lobby">The data of the lobby where the game will be started.</param>
	/// <param name="sender">The ID of the sender starting the game.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeStartGame(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after starting a game.
	/// </summary>
	/// <param name="lobby">The data of the lobby where the game has started.</param>
	/// <param name="sender">The ID of the sender who started the game.</param>
	public virtual void AfterStartGame(Dictionary<string, Variant> lobby, long sender) { }

	/// <summary>
	/// Executed before loading lobby players.
	/// </summary>
	/// <param name="sender">The ID of the sender requesting lobby players.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeLoadLobbyPlayers(long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterLoadLobbyPlayers(long sender) { }

	/// <returns>
	/// dictionary contains "Status" and "Message", when status false, execution will be terminated
	/// and error event with "Message" will be emmited to client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeSpawnNode(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after loading lobby players.
	/// </summary>
	/// <param name="sender">The ID of the sender who requested lobby players.</param>
	public virtual void AfterSpawnNode(Dictionary<string, Variant> lobby, long sender) { }

	/// <summary>
	/// Executed before spawning a node.
	/// </summary>
	/// <param name="lobby">The data of the lobby where the node will be spawned.</param>
	/// <param name="sender">The ID of the sender requesting the node spawn.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeSpawnLobbyNodes(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after spawning a node.
	/// </summary>
	/// <param name="lobby">The data of the lobby where the node was spawned.</param>
	/// <param name="sender">The ID of the sender who requested the node spawn.</param>
	public virtual void AfterSpawnLobbyNodes(Dictionary<string, Variant> lobby, long sender) { }

	/// <summary>
	/// Executed before spawning lobby nodes.
	/// </summary>
	/// <param name="lobby">The data of the lobby where nodes will be spawned.</param>
	/// <param name="sender">The ID of the sender requesting the nodes spawn.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeDespawnNode(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after spawning lobby nodes.
	/// </summary>
	/// <param name="lobby">The data of the lobby where nodes were spawned.</param>
	/// <param name="sender">The ID of the sender who requested the nodes spawn.</param>
	public virtual void AfterDespawnNode(Dictionary<string, Variant> lobby, long sender) { }

	/// <summary>
	/// Executed before despawning removed scene nodes.
	/// </summary>
	/// <param name="lobbyId">The ID of the lobby where the nodes will be despawned.</param>
	/// <param name="sender">The ID of the sender requesting the nodes despawn.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeDespawnRemovedSceneNodes(int lobbyId, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after despawning removed scene nodes.
	/// </summary>
	/// <param name="lobbyId">The ID of the lobby where the nodes were despawned.</param>
	/// <param name="sender">The ID of the sender who requested the nodes despawn.</param>
	public virtual void AfterDespawnRemovedSceneNodes(int lobbyId, long sender) { }

	/// <summary>
	/// Executed before joining a team.
	/// </summary>
	/// <param name="sender">The ID of the sender requesting to join a team.</param>
	/// <param name="teamId">The ID of the team to join.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeJoinTeam(long sender, Variant teamId)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after joining a team.
	/// </summary>
	/// <param name="sender">The ID of the sender who joined the team.</param>
	/// <param name="teamId">The ID of the team that was joined.</param>
	public virtual void AfterJoinTeam(long sender, Variant teamId) { }

	/// <summary>
	/// Executed before sending a global message.
	/// </summary>
	/// <param name="sender">The ID of the sender sending the message.</param>
	/// <param name="message">The message to be sent.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeSendGlobalMessage(long sender, string message)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after sending a global message.
	/// </summary>
	/// <param name="sender">The ID of the sender who sent the message.</param>
	/// <param name="message">The message that was sent.</param>
	public virtual void AfterSendGlobalMessage(long sender, string message) { }

	/// <summary>
	/// Executed before sending a lobby message.
	/// </summary>
	/// <param name="lobby">The data of the lobby where the message will be sent.</param>
	/// <param name="sender">The ID of the sender sending the message.</param>
	/// <param name="message">The message to be sent.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeSendLobbyMessage(Dictionary<string, Variant> lobby, long sender, string message)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after sending a lobby message.
	/// </summary>
	/// <param name="lobby">The data of the lobby where the message was sent.</param>
	/// <param name="sender">The ID of the sender who sent the message.</param>
	/// <param name="message">The message that was sent.</param>
	public virtual void AfterSendLobbyMessage(Dictionary<string, Variant> lobby, long sender, string message) { }


	/// <summary>
	/// Executed before sending a team message.
	/// </summary>
	/// <param name="lobby">The data of the lobby where the message will be sent.</param>
	/// <param name="teamId">The ID of the team receiving the message.</param>
	/// <param name="sender">The ID of the sender sending the message.</param>
	/// <param name="message">The message to be sent.</param>
	/// <returns>
	/// Dictionary containing "Status" and "Message". If "Status" is false, execution will be terminated
	/// and an error event with "Message" will be emitted to the client.
	/// </returns>
	public virtual Dictionary<string, Variant> BeforeSendTeamMessage(Dictionary<string, Variant> lobby, int teamId, long sender, string message)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}

	/// <summary>
	/// Executed after sending a team message.
	/// </summary>
	/// <param name="lobby">The data of the lobby where the message was sent.</param>
	/// <param name="teamId">The ID of the team that received the message.</param>
	/// <param name="sender">The ID of the sender who sent the message.</param>
	/// <param name="message">The message that was sent.</param>
	public virtual void AfterSendTeamMessage(Dictionary<string, Variant> lobby, int teamId, long sender, string message) { }

	/// <summary>
	/// Executed before removing a player from a lobby.
	/// </summary>
	/// <param name="playerId">The ID of the player being removed.</param>	
	public virtual void BeforeRemovePlayerFromLobby(long playerId) { }

	/// <summary>
	/// Executed after removing a player from a lobby.
	/// </summary>
	/// <param name="playerId">The ID of the player who was removed.</param>
	public virtual void AfterRemovePlayerFromLobby(long playerId) { }

	/// <summary>
	/// Executed before registering a player.
	/// </summary>
	/// <param name="id">The ID of the player being registered.</param>
	public virtual void BeforeRegisterPlayer(long id) { }

	/// <summary>
	/// Executed after registering a player.
	/// </summary>
	/// <param name="id">The ID of the player who was registered.</param>
	public virtual void AfterRegisterPlayer(long id) { }

	/// <summary>
	/// Executed before unregistering a player.
	/// </summary>
	/// <param name="id">The ID of the player being unregistered.</param>
	public virtual void BeforeUnRegisterPlayer(long id) { }

	/// <summary>
	/// Executed after unregistering a player.
	/// </summary>
	/// <param name="id">The ID of the player who was unregistered.</param>
	public virtual void AfterUnRegisterPlayer(long id) { }
}
