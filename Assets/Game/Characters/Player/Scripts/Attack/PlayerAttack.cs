﻿using Mirror;
using UnityEngine;

public partial class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private float attackDelay = .25f;
    [SerializeField] private float cooldownDuration = 1f;
    private float _cooldown;
   

    private void Update()
    {
        _cooldown -= Time.deltaTime;
    }
}