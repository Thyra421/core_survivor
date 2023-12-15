using Mirror;
using UnityEngine;

public partial class PlayerAttack
{
    [Header("Server")] [SerializeField] private float attackRadius;
    [SerializeField] private float attackDistance;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackDistance, attackRadius);
    }

    [Server]
    private void ServerAttack()
    {
        ConsoleLogger.Server("Attack");

        Ray ray = new(transform.position + transform.forward * attackDistance,
            transform.position + transform.forward * (attackDistance + .1f));
        RaycastHit[] hits = Physics.SphereCastAll(ray, attackRadius);

        if (hits.Length <= 0) return;

        foreach (RaycastHit hit in hits)
            if (hit.transform.TryGetComponent(out IDamageable enemy))
                enemy.TakeDamage(25);
    }
}