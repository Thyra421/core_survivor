using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class PlayerManager : NetworkSingleton<PlayerManager>
{
    public Listenable<Player> LocalPlayer { get; } = new();

    public ListenableList<Player> Players { get; } = new();

    public Listenable<int> PlayersAlive { get; } = new();

    [ClientRpc]
    public void SetPlayersAliveRpc(int value)
    {
        PlayersAlive.Value = value;
    }

    protected override void Awake()
    {
        Players.OnChanged += players => PlayersAlive.Value = players.Count;
    }
}