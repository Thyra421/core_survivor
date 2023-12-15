using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public partial class Player : NetworkBehaviour
{
    public PlayerHealth Health { get; private set; }

    private void Awake()
    {
        Health = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        PlayerManager.Current.Players.Add(this);
    }

    private void OnDestroy()
    {
        PlayerManager.Current.Players.Remove(this);
    }
}
