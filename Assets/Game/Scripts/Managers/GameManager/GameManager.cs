using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public partial class GameManager : NetworkSingleton<GameManager>
{
    [SyncVar(hook = nameof(TimerHook))] private int _timerSync;
    public readonly Listenable<int> timer = new();

    private void TimerHook(int oldValue, int newValue)
    {
        _timerSync = newValue;
        timer.Value = newValue;
    }

    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}