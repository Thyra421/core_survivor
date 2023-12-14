using JetBrains.Annotations;
using Mirror;
using UnityEngine;

public partial class Enemy : NetworkBehaviour
{
    [CanBeNull] public Transform Target { get; private set; }

    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}