using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerMovement))]
public partial class PlayerAttack
{
    private PlayerAnimation _playerAnimation;
    private PlayerMovement _playerMovement;

    [ClientRpc]
    private void ClientAttack()
    {
        _playerAnimation.SetTrigger("Attack");

        if (!isOwned) return;

        _playerMovement.Dash();
        _cooldown = cooldownDuration;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void OnAttack()
    {
        if (!isClient || !isOwned) return;

        if (_cooldown > 0) return;

        AttackCommand();
    }
}