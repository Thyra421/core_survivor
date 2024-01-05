using System.Collections;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyAnimation))]
public partial class EnemyAttack
{
    [SerializeField]
    private float attackRange;

    [SerializeField]
    private int attackDamage;

    [SerializeField]
    private float attackDelay;

    [SerializeField]
    private Cooldown cooldown;

    private Enemy _enemy;
    private EnemyAnimation _enemyAnimation;

    private bool IsTargetInRange => _enemy.Target != null &&
                                    Vector3.Distance(transform.position, _enemy.Target!.position) < attackRange;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.up, attackRange);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _enemy = GetComponent<Enemy>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
    }

    [Server]
    private void ServerUpdate()
    {
        if (_enemy.IsDead) return;

        cooldown.Update();

        if (cooldown.IsReady && IsTargetInRange)
            Attack();
    }

    [Server]
    private void Attack()
    {
        _enemyAnimation.ServerSetTrigger("Attack");
        StartCoroutine(AttackCoroutine());
        cooldown.Start();
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);

        if (!IsTargetInRange) yield break;

        IDamageable damageable = _enemy.Target!.GetComponent<IDamageable>();
        damageable.TakeDamage(attackDamage);
    }
}