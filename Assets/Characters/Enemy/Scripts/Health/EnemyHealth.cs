using System.Collections;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(EnemyAnimation))]
public class EnemyHealth : CharacterHealth
{
    private EnemyAnimation _enemyAnimation;
    private CapsuleCollider _capsuleCollider;

    public override void OnStartServer()
    {
        base.OnStartServer();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    [Server]
    public override void Die()
    {
        _enemyAnimation.SetTrigger("Die");
        _capsuleCollider.enabled = false;
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(8);
        NetworkServer.Destroy(gameObject);
    }
}