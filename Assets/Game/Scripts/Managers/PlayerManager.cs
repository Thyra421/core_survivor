using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class PlayerManager : NetworkSingleton<PlayerManager>
{
    public Listenable<Player> LocalPlayer { get; } = new();

    public ListenableList<Player> Players { get; } = new();

    public Listenable<int> PlayersAlive { get; } = new();

    [ClientRpc]
    private void SetPlayersAliveRpc(int value)
    {
        PlayersAlive.Value = value;
    }

    [Server]
    public void PlayerDie()
    {
        PlayersAlive.Value--;
        SetPlayersAliveRpc(PlayersAlive.Value);
        if (PlayersAlive.Value == 0)
            GameManager.Current.GameOver("Tous les joueurs sont morts !");
    }

    protected override void Awake()
    {
        base.Awake();
        Players.OnChanged += players => PlayersAlive.Value = players.Count;
    }
}