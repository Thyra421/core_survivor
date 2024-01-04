using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public partial class GameManager : NetworkSingleton<GameManager>
{
    public readonly Listenable<float> timer = new();

    private void Update()
    {
        timer.Value -= Time.deltaTime;

        if (isServer)
            ServerUpdate();
    }
}