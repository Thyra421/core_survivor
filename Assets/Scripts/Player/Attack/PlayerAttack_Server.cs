using Mirror;
using UnityEngine;

public partial class PlayerAttack
{
    [Header("Server")] [SerializeField] private float attackRadius;
    [SerializeField] private float attackDistance;

    [Command]
    private void AttackCommand()
    {
        Log.Server("Attack");
        
        Attack();

        // Gizmos.DrawWireSphere(transform.position + transform.forward * attackDistance, attackRadius);
        //
        // Ray ray = new Ray(transform.position, transform.forward);
        // RaycastHit[] hits = Physics.SphereCastAll(ray, attackRadius);
        //
        // if (hits.Length > 0)
        //     foreach (RaycastHit hit in hits)
        //         Log.Server(hit.transform.name);
    }
}