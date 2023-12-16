using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public partial class Player : NetworkBehaviour
{
    public PlayerHealth Health { get; private set; }
    public PlayerMovement Movement { get; private set; }

    private void Awake()
    {
        Health = GetComponent<PlayerHealth>();
        Movement = GetComponent<PlayerMovement>();
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