using System;
using System.Collections;
using Mirror;
using UnityEngine;

public partial class PlayerAttack
{
    [Header("Server")]
    [SerializeField]
    private float attackRadius;

    [SerializeField]
    private float attackDistance;

    [SerializeField]
    private float attackDuration;

    [SyncVar(hook = nameof(EnergyHook))]
    private int energySync;

    private Vector3 AttackPosition => transform.position + transform.up + transform.forward * attackDistance;

    public bool IsAttacking { get; private set; }
    public Vector3 AttackPoint { get; private set; }
    public Listenable<int> Energy { get; } = new();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosition, attackRadius);
    }

    private void EnergyHook(int oldValue, int newValue)
    {
        energySync = newValue;
        Energy.Value = newValue;
    }

    [Command]
    private void AttackCommand(Vector3 direction)
    {
        if (!Cooldown.IsReady) return;

        AttackPoint = direction;

        ClientAttack();
        ServerAttack();
    }

    [Command]
    private void ThrowCommand(Vector3 direction)
    {
        if (Energy.Value != 100) return;

        AttackPoint = direction;

        ClientAttack();
        ServerAttack();
    }

    [Server]
    private void ServerAttack()
    {
        Cooldown.Start();
        IsAttacking = true;

        StartCoroutine(ServerAttackCoroutine());
        StartCoroutine(IsAttackingCoroutine());
    }
    
    [Server]
    private void ServerThrow()
    {
        Energy.Value = 0;
        IsAttacking = true;

        StartCoroutine(ServerAttackCoroutine());
        StartCoroutine(IsAttackingCoroutine());
    }

    [Server]
    private IEnumerator IsAttackingCoroutine()
    {
        yield return new WaitForSeconds(attackDuration);

        IsAttacking = false;
    }

    [Server]
    private IEnumerator ServerAttackCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);

        Collider[] hits = Physics.OverlapSphere(AttackPosition, attackRadius);

        if (hits.Length <= 0) yield return null;

        foreach (Collider c in hits)
            if (c.transform.TryGetComponent(out EnemyHealth damageable)) {
                damageable.TakeDamage(25);
                energySync = Math.Clamp(energySync + 1, 0, 100);
            }
    }
}