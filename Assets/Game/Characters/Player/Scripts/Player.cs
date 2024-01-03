using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerAnimation))]
public partial class Player : NetworkBehaviour
{
    public PlayerHealth Health { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerAnimation Animation { get; private set; }
    public PlayerClass Class { get; private set; }

    private void Awake()
    {
        Health = GetComponent<PlayerHealth>();
        Movement = GetComponent<PlayerMovement>();
        Animation = GetComponent<PlayerAnimation>();
        Class = GetComponent<PlayerClass>();
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