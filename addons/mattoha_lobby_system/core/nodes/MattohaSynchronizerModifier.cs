using Godot;
using Godot.Collections;
using Mattoha.Core.Utils;

namespace Mattoha.Nodes;
public partial class MattohaSynchronizerModifier : MultiplayerSynchronizer
{
    [Export] public bool ReplicateForTeamOnly { get; set; } = false;


    public override void _Ready()
    {

        PublicVisibility = false;
        MattohaSystem.Instance.Server.PlayerJoinedLobby += OnPlayerJoinedLobby_Server;
        MattohaSystem.Instance.Server.PlayerLeftLobby += OnPlayerLeftLobby_Server;
        MattohaSystem.Instance.Client.NewPlayerJoined += OnNewPlayerJoinedLobby;
        MattohaSystem.Instance.Client.PlayerLeft += OnPlayerLeftLobby;
        MattohaSystem.Instance.Client.JoinedPlayerUpdated += OnJoinedPlayerUpdated;

        ApplySetVisibilityFor();
        ApplyMattohaReplicationFilter();
        base._Ready();
    }

    private void OnJoinedPlayerUpdated(Dictionary<string, Variant> playerData)
    {
        ApplySetVisibilityFor();
    }


    private void OnPlayerLeftLobby(Dictionary<string, Variant> playerData)
    {
        ApplySetVisibilityFor();
    }


    private void OnNewPlayerJoinedLobby(Dictionary<string, Variant> playerData)
    {
        ApplySetVisibilityFor();
    }


    private void OnPlayerLeftLobby_Server(long playerId, int lobbyId)
    {
        ApplySetVisibilityFor();
    }


    private void OnPlayerJoinedLobby_Server(long playerId, int lobbyId)
    {
        ApplySetVisibilityFor();
    }


    private void ApplySetVisibilityFor()
    {
        if (Multiplayer.IsServer())
        {
            var players = MattohaSystem.Instance.Server.GetLobbyPlayers(MattohaSystem.ExtractLobbyId(GetPath()));
            foreach (var player in players)
            {
                var lobbyId = MattohaSystem.ExtractLobbyId(GetPath());
                var visible = player[MattohaPlayerKeys.IsInGame].AsBool()
                                && player[MattohaPlayerKeys.JoinedLobbyId].AsInt32() == lobbyId
                                && player[MattohaPlayerKeys.IsInGame].AsBool();
                SetVisibilityFor(player[MattohaPlayerKeys.Id].AsInt32(), visible);
            }
        }
        else
        {
            var players = MattohaSystem.Instance.Client.CurrentLobbyPlayers.Values;
            SetVisibilityFor(1, true);
            foreach (var player in players)
            {
                var visible = player[MattohaPlayerKeys.IsInGame].AsBool() && (!ReplicateForTeamOnly || MattohaSystem.Instance.Client.IsPlayerInMyTeam(player[MattohaPlayerKeys.Id].AsInt32()));
                SetVisibilityFor(player[MattohaPlayerKeys.Id].AsInt32(), visible);
            }
        }
    }

    public virtual void ApplyMattohaReplicationFilter()
    {
        var mattohaFilter = new Callable(this, nameof(MattohaFilter));
        AddVisibilityFilter(mattohaFilter);
    }

    private bool MattohaFilter(long peerId)
    {
        if (Multiplayer.IsServer())
        {
            if (peerId == 1 || peerId == 0) return false;
            var player = MattohaSystem.Instance.Server.GetPlayer(peerId);
            if (player == null)
                return false;
            var replicate = player[MattohaPlayerKeys.IsInGame].AsBool() == true;
            return replicate;
        }
        if (ReplicateForTeamOnly && !Multiplayer.IsServer())
        {
            if (peerId == 1 || peerId == 0) return true;
            var replicate = MattohaSystem.Instance.Client.CanReplicate && MattohaSystem.Instance.Client.IsPlayerInMyTeam(peerId) && MattohaSystem.Instance.Client.IsPlayerInGame(peerId);
            return replicate;
        }
        else if (!ReplicateForTeamOnly && !Multiplayer.IsServer())
        {
            if (peerId == 1 || peerId == 0) return true;
            var replicate = MattohaSystem.Instance.Client.CanReplicate && MattohaSystem.Instance.Client.IsPlayerInLobby(peerId) && MattohaSystem.Instance.Client.IsPlayerInGame(peerId);
            return replicate;
        }

        return false;
    }

    public override void _ExitTree()
    {

        MattohaSystem.Instance.Server.PlayerJoinedLobby -= OnPlayerJoinedLobby_Server;
        MattohaSystem.Instance.Server.PlayerLeftLobby -= OnPlayerLeftLobby_Server;
        MattohaSystem.Instance.Client.NewPlayerJoined -= OnNewPlayerJoinedLobby;
        MattohaSystem.Instance.Client.PlayerLeft -= OnPlayerLeftLobby;
        MattohaSystem.Instance.Client.JoinedPlayerUpdated -= OnJoinedPlayerUpdated;
        base._ExitTree();
    }
}