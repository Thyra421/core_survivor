using System;
using System.Collections;
using Mirror;
using UnityEngine;

public partial class PlayerAttack
{
    [Header("Server")] [SerializeField] private float attackRadius;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackDuration;

    private Vector3 AttackPosition => transform.position + transform.up + transform.forward * attackDistance;

    public bool IsAttacking { get; private set; }
    public Vector3 AttackDirection { get; private set; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosition, attackRadius);
    }

    [Command]
    private void AttackCommand(Vector3 direction)
    {
        if (_cooldown > 0) return;

        AttackDirection = direction;

        ClientAttack();
        ServerAttack();
    }

    [Server]
    private void ServerAttack()
    {
        _cooldown = cooldownDuration;
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
            if (c.transform.TryGetComponent(out EnemyHealth damageable))
                damageable.TakeDamage(25);
    }
}