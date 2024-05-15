namespace MattohaLobbySystem.Core.Interfaces;

public interface IMattohaLobby : IMattohaPrivateProps
{
	public long Id { get; set; }
	public long OwnerId { get; set; }
	public string Name { get; set; }
	public int MaxPlayers { get; set; }
	public int PlayersCount { get; set; }
	public bool IsGameStarted { get; set; }
}
