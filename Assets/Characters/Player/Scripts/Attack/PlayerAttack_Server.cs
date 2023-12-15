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
        
        Ray ray = new(transform.position + transform.forward * attackDistance,
            transform.position + transform.forward * (attackDistance + .1f));
        RaycastHit[] hits = Physics.SphereCastAll(ray, attackRadius);

        if (hits.Length <= 0) return;

        foreach (RaycastHit hit in hits)
            if (hit.transform.TryGetComponent(out EnemyHealth damageable))
                damageable.TakeDamage(25);
    }
}