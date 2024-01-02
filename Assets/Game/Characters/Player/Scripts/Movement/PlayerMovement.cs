using Mirror;
using UnityEngine;

public partial class PlayerMovement : NetworkBehaviour
{
    [Header("Dash")]
    [SerializeField]
    private float dashCooldownDuration;

    public Cooldown DashCooldown { get; private set; }

    private void Awake()
    {
        DashCooldown = new Cooldown(dashCooldownDuration);
    }

    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}