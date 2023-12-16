using Mirror;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimation))]
[RequireComponent(typeof(CapsuleCollider))]
public partial class EnemyHealth
{
    private EnemyAnimation _enemyAnimation;
    private CapsuleCollider _capsuleCollider;

    public override void OnStartClient()
    {
        base.OnStartClient();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    [ClientRpc]
    private void ClientDie()
    {
        _enemyAnimation.ClientSetTrigger("Die");
        _capsuleCollider.enabled = false;
    }
}