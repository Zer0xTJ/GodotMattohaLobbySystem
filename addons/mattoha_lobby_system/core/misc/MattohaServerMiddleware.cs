using Godot;
using Godot.Collections;

namespace Mattoha.Misc;

public partial class MattohaServerMiddleware: Node
{
	public virtual Dictionary<string, Variant> BeforeSetPlayerData (Dictionary<string, Variant> payload, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterSetPlayerData(Dictionary<string, Variant> payload, long sender) { }

	
	public virtual Dictionary<string, Variant> BeforeCreateLobby(Dictionary<string, Variant> lobbyData, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterCreateLobby(Dictionary<string, Variant> lobbyData, long sender) { }

	
	public virtual Dictionary<string, Variant> BeforeSetLobbyData(int lobbyId, Dictionary<string, Variant> payload, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterSetLobbyData(int lobbyId, Dictionary<string, Variant> payload, long sender) { }

	
	public virtual  Dictionary<string, Variant> BeforeSetLobbyOwner(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterSetLobbyOwner(Dictionary<string, Variant> lobby, long sender) { }

	
	public virtual Dictionary<string, Variant> BeforeLoadAvailableLobbies(long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterLoadAvailableLobbies(long sender) { }


	public virtual Dictionary<string, Variant> BeforeJoinLobby(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterJoinLobby(Dictionary<string, Variant> lobby, long sender) { }

	
	public virtual Dictionary<string, Variant> BeforeStartGame(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterStartGame(Dictionary<string, Variant> lobby, long sender) { }

	
	public virtual Dictionary<string, Variant> BeforeLoadLobbyPlayers(long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterLoadLobbyPlayers(long sender) { }

	
	public virtual Dictionary<string, Variant> BeforeSpawnNode(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterSpawnNode(Dictionary<string, Variant> lobby, long sender) { }


	public virtual Dictionary<string, Variant> BeforeSpawnLobbyNodes(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterSpawnLobbyNodes(Dictionary<string, Variant> lobby, long sender) { }

	
	public virtual Dictionary<string, Variant> BeforeDespawnNode(Dictionary<string, Variant> lobby, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterDespawnNode(Dictionary<string, Variant> lobby, long sender) { }

	
	public virtual Dictionary<string, Variant> BeforeDespawnRemovedSceneNodes(int lobbyId, long sender)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterDespawnRemovedSceneNodes(int lobbyId, long sender) { }


	public virtual Dictionary<string, Variant> BeforeJoinTeam(long sender, Variant teamId)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterJoinTeam(long sender, Variant teamId) { }


	public virtual Dictionary<string, Variant> BeforeSendGlobalMessage(long sender, string message)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterSendGlobalMessage(long sender, string message) { }

	
	public virtual Dictionary<string, Variant> BeforeSendLobbyMessage(Dictionary<string, Variant> lobby, long sender, string message)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterSendLobbyMessage(Dictionary<string, Variant> lobby, long sender, string message) { }

	
	public virtual Dictionary<string, Variant> BeforeSendTeamMessage(Dictionary<string, Variant> lobby, int teamId, long sender, string message)
	{
		return new()
		{
			{ "Status", true },
			{ "Message", "" }
		};
	}
	public virtual void AfterSendTeamMessage(Dictionary<string, Variant> lobby, int teamId, long sender, string message) { }
	
	
	public virtual void BeforeRemovePlayerFromLobby(long playerId) { }
	public virtual void AfterRemovePlayerFromLobby(long playerId) {  }

	
	public virtual void BeforeRegisterPlayer(long id) { }
	public virtual void AfterRegisterPlayer(long id) { }

	
	public virtual void BeforeUnRegisterPlayer(long id) { }
	public virtual void AfterUnRegisterPlayer(long id) { }
}
