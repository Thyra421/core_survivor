using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T>
{
    public static T Current { get; private set; }

    protected virtual void Awake()
    {
        if (Current == null)
            Current = this as T;
        else
            Destroy(gameObject);
    }
}