using System.Collections;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public partial class PlayerAttack
{
    [Header("Server")] [SerializeField] private float attackRadius;
    [SerializeField] private float attackDistance;
    private PlayerHealth _playerHealth;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackDistance, attackRadius);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _playerHealth = GetComponent<PlayerHealth>();
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

        Vector3 attackPosition = transform.position + transform.forward * attackDistance;
        Collider[] hits = Physics.OverlapSphere(attackPosition, attackRadius);

        if (hits.Length <= 0) yield return null;

        foreach (Collider c in hits)
            if (c.transform.TryGetComponent(out EnemyHealth damageable))
                damageable.TakeDamage(25);
    }
}