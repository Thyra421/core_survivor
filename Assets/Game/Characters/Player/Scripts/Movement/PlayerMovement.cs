using Mirror;
using UnityEngine;

public partial class PlayerMovement : NetworkBehaviour
{
    [Header("Dash")]

    [SerializeField]
    private Cooldown dashCooldown;

    public Cooldown DashCooldown => dashCooldown;

    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}