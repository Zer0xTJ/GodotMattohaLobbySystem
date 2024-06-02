using Godot;
using Godot.Collections;
using Mattoha.Core.Utils;
using System;

namespace Mattoha.Nodes;
public partial class MattohaSynchronizerModifier : Node
{
    private MultiplayerSynchronizer _synchronizer;
    [Export] public bool ReplicateForTeamOnly { get; set; } = false;


    public override void _Ready()
    {
        _synchronizer = GetParent<MultiplayerSynchronizer>();
        _synchronizer.PublicVisibility = false;
        if (Multiplayer.IsServer())
            return;
        MattohaSystem.Instance.Client.LoadLobbyPlayersSucceed += OnLoadLobbyPlayers;
        MattohaSystem.Instance.Client.NewPlayerJoined += OnPlayerJoined;
        MattohaSystem.Instance.Client.JoinedPlayerUpdated += OnJoinedPLayerUpdated;
        MattohaSystem.Instance.Client.PlayerChangedHisTeam += OnPlayerChangedHisTeam;
		MattohaSystem.Instance.Client.PlayerLeft += OnPlayerLeft;
        ApplyMattohaReplicationFilter();
        base._Ready();
    }


	private void SetupReplicationVisibility()
    {
        foreach (var player in MattohaSystem.Instance.Client.CurrentLobbyPlayers.Values)
        {
            var playerId = player[MattohaPlayerKeys.Id].AsInt32();
            var isInGame = player[MattohaPlayerKeys.IsInGamae].AsBool();
            var isSameTeam = MattohaSystem.Instance.Client.IsPlayerInMyTeam(playerId);
            _synchronizer.SetVisibilityFor(playerId, isInGame && (isSameTeam && ReplicateForTeamOnly || !ReplicateForTeamOnly));
        }
        _synchronizer.SetVisibilityFor(1, true);
    }

	private void OnPlayerLeft(Dictionary<string, Variant> playerData)
	{
        SetupReplicationVisibility();
	}


    private void OnPlayerChangedHisTeam(Dictionary<string, Variant> playerData)
    {
        SetupReplicationVisibility();
    }


    private void OnJoinedPLayerUpdated(Dictionary<string, Variant> playerData)
    {
        SetupReplicationVisibility();
    }


    private void OnPlayerJoined(Dictionary<string, Variant> playerData)
    {
        SetupReplicationVisibility();
    }


    private void OnLoadLobbyPlayers(Array<Dictionary<string, Variant>> players)
    {
        SetupReplicationVisibility();
    }


    public virtual void ApplyMattohaReplicationFilter()
    {
        var mattohaFilter = new Callable(this, nameof(MattohaFilter));
        _synchronizer.AddVisibilityFilter(mattohaFilter);
    }


    private bool MattohaFilter(long peerId)
    {
        if (peerId == 1) return true;
        if (ReplicateForTeamOnly)
        {
            return MattohaSystem.Instance.Client.CanReplicate && MattohaSystem.Instance.Client.IsPlayerInMyTeam(peerId) && MattohaSystem.Instance.Client.IsPlayerInGame(peerId);
        }
        else
        {
            return MattohaSystem.Instance.Client.CanReplicate && MattohaSystem.Instance.Client.IsPlayerInLobby(peerId) && MattohaSystem.Instance.Client.IsPlayerInGame(peerId);
        }
    }

    public override void _ExitTree()
    {
        MattohaSystem.Instance.Client.LoadLobbyPlayersSucceed -= OnLoadLobbyPlayers;
        MattohaSystem.Instance.Client.NewPlayerJoined -= OnPlayerJoined;
        MattohaSystem.Instance.Client.JoinedPlayerUpdated -= OnJoinedPLayerUpdated;
        MattohaSystem.Instance.Client.PlayerChangedHisTeam -= OnPlayerChangedHisTeam;
		MattohaSystem.Instance.Client.PlayerLeft -= OnPlayerLeft;
        base._ExitTree();
    }
}