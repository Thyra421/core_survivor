﻿using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public partial class PlayerAttack
{
    private PlayerAnimation _playerAnimation;

    [ClientRpc]
    private void ClientAttack()
    {
        _playerAnimation.SetTrigger("Attack");

        if (!isOwned) return;

        _cooldown = cooldownDuration;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void OnAttack()
    {
        if (!isClient || !isOwned) return;

        if (_cooldown > 0) return;

        AttackCommand();
    }
}