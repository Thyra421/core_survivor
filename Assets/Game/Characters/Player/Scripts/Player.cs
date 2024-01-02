using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerAttack))]
public partial class Player : NetworkBehaviour
{
    public PlayerHealth Health { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }

    private void Awake()
    {
        Health = GetComponent<PlayerHealth>();
        Movement = GetComponent<PlayerMovement>();
        Attack = GetComponent<PlayerAttack>();
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