using Godot;
using MattohaLobbySystem.Core.Models;
using MattohaLobbySystem.Core.Nodes;
using System.Text.Json.Nodes;
namespace MattohaLobbySystem.Core;

public partial class MattohaServerMiddleware : Node
{
	/// <summary>
	/// Executes before setting player data.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">player data to be set</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeSetPlayerData(MattohaServerBase server, JsonObject playerData)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after setting player data.
	/// </summary>
	/// <param name="playerData">player data</param>
	/// <param name="server">server instance</param>
	public virtual void AfterSetPlayerData(MattohaServerBase server, JsonObject playerData) { }


	/// <summary>
	/// Executes before createing lobby.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="lobbyData">Lobby data as JsonObject that will be used to create a lobby</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeCreateLobby(MattohaServerBase server, JsonObject lobbyData)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after create lobby.
	/// </summary>
	/// <param name="lobbyData">lobbyData data</param>
	/// <param name="server">server instance</param>
	public virtual void AfterCreateLobby(MattohaServerBase server, JsonObject lobbyData) { }


	/// <summary>
	/// Executes before a player join a lobby.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player data as JsonObject</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeJoinLobby(MattohaServerBase server, JsonObject playerData, JsonObject lobbyData)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after a player joined a lobby.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player data as JsonObject</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	public virtual void AfterJoinLobby(MattohaServerBase server, JsonObject playerData, JsonObject lobbyData) { }


	/// <summary>
	/// Executes before setting lobby data by owner.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="ownerData">Owner data as JsonObject</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeSetLobbyData(MattohaServerBase server, JsonObject ownerData, JsonObject lobbyData)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after lobby data being set.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player data as JsonObject</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	public virtual void AfterSetLobbyData(MattohaServerBase server, JsonObject playerData, JsonObject lobbyData) { }


	/// <summary>
	/// Executes before sending a team message.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player data as JsonObject</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeSendTeamMessage(MattohaServerBase server, JsonObject playerData)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after a team message sent.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player data as JsonObject</param>
	public virtual void AfterSendTeamMessage(MattohaServerBase server, JsonObject playerData) { }



	/// <summary>
	/// Executes before sending a lobby message.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player data as JsonObject</param>
	/// <param name="lobbyData">The lobby data a message will be broadcasted for.</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeSendLobbyMessage(MattohaServerBase server, JsonObject playerData, JsonObject lobbyData)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after a lobby message sent.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player data as JsonObject</param>
	/// <param name="lobbyData">The lobby data a message sent to.</param>
	public virtual void AfterSendLobbyMessage(MattohaServerBase server, JsonObject playerData, JsonObject lobbyData) { }


	/// <summary>
	/// Executes before sending a global message.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player data as JsonObject</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeSendGlobalMessage(MattohaServerBase server, JsonObject playerData)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after a global message sent.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player data as JsonObject</param>
	public virtual void AfterSendGlobalMessage(MattohaServerBase server, JsonObject playerData) { }



	/// <summary>
	/// Executes before starting lobby game.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="ownerData">Owner of the lobby as JsonObject, null if start game requested from server.</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeStartGame(MattohaServerBase server, JsonObject? ownerData, JsonObject lobbyData)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after starting the game.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="ownerData">Owner of the lobby as JsonObject, null if start game requested from server.</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	public virtual void AfterStartGame(MattohaServerBase server, JsonObject? ownerData, JsonObject lobbyData) { }


	/// <summary>
	/// Executes before ending lobby game.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="ownerData">Owner of the lobby as JsonObject, null if end game requested from server.</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeEndGame(MattohaServerBase server, JsonObject? ownerData, JsonObject lobbyData)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after ending the game.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="ownerData">Owner of the lobby as JsonObject, null if end game requested from server.</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	public virtual void AfterEndGame(MattohaServerBase server, JsonObject? ownerData, JsonObject lobbyData) { }


	/// <summary>
	/// Executes before player leave lobby.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">player data as JsonObject</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	public virtual void BeforePlayerLeaveLobby(MattohaServerBase server, JsonObject playerData, JsonObject lobbyData) { }


	/// <summary>
	/// Executes after player leave lobby.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">player data as JsonObject</param>
	/// <param name="lobbyData">Lobby data as JsonObject</param>
	public virtual void AfterPlayerLeaveLobby(MattohaServerBase server, JsonObject playerData, JsonObject lobbyData) { }


	/// <summary>
	/// Executes before spawning a node.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player as JsonObject</param>
	/// <param name="lobbyData">lobby data that node will be spawn in as JsonObject</param>
	/// <param name="nodeInfo">node info that will be spawned</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeSpawnNode(MattohaServerBase server, JsonObject playerData, JsonObject lobbyData, MattohaSpawnNodeInfo nodeInfo)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after spawning a node.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="playerData">Player as JsonObject</param>
	/// <param name="lobbyData">lobby data that node will be spawn in as JsonObject</param>
	/// <param name="nodeInfo">node info that will be spawned</param>
	public virtual void AfterSpawnNode(MattohaServerBase server, JsonObject playerData, JsonObject lobbyData, MattohaSpawnNodeInfo nodeInfo) { }


	/// <summary>
	/// Executes before despawning a node.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="byPlayerData">Player who requested the despawn JsonObject</param>
	/// <param name="lobbyData">lobby data of the node as JsonObject</param>
	/// <param name="nodeInfo">node info that will be despawned</param>
	/// <returns>MattohaMiddlewareResponse object with status, if status true then execution will continue.</returns>
	public virtual MattohaMiddlewareResponse BeforeDespawnNode(MattohaServerBase server, JsonObject byPlayerData, JsonObject lobbyData, MattohaSpawnNodeInfo nodeInfo)
	{
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}


	/// <summary>
	/// Executes after despawning a node.
	/// </summary>
	/// <param name="server">server instance</param>
	/// <param name="byPlayerData">Player who requested the despawn JsonObject</param>
	/// <param name="lobbyData">lobby data of the node as JsonObject</param>
	/// <param name="nodeInfo">node info that will be despawned</param>
	public virtual void AfterDespawnNode(MattohaServerBase server, JsonObject byPlayerData, JsonObject lobbyData, MattohaSpawnNodeInfo nodeInfo) { }

}
