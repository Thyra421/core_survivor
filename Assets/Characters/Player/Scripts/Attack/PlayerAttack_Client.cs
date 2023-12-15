using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public partial class PlayerAttack
{
    private PlayerAnimation _playerAnimation;

    [ClientRpc]
    private void ClientAttack()
    {
        _playerAnimation.SetTrigger("Attack");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void OnAttack()
    {
        if (isClient && isOwned)
            AttackCommand();
    }
}