using System.Collections;
using Mirror;
using UnityEngine;

public partial class PlayerAttack
{
    [Header("Server")] [SerializeField] private float attackRadius;
    [SerializeField] private float attackDistance;

    private Vector3 AttackPosition => transform.position + transform.up + transform.forward * attackDistance;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosition, attackRadius);
    }

    [Server]
    private void ServerAttack()
    {
        _cooldown = cooldownDuration;

        StartCoroutine(ServerAttackCoroutine());
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