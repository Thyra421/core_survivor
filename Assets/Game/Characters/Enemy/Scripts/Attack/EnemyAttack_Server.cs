using System.Collections;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyAnimation))]
public partial class EnemyAttack
{
    [SerializeField] private float attackRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackCooldownDuration;
    [SerializeField] private float attackDelay;

    private Enemy _enemy;
    private EnemyAnimation _enemyAnimation;
    private float _cooldown;

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
        
        _cooldown -= Time.deltaTime;

        if (_cooldown <= 0 && IsTargetInRange)
            Attack();
    }

    [Server]
    private void Attack()
    {
        _enemyAnimation.ServerSetTrigger("Attack");
        StartCoroutine(AttackCoroutine());
        _cooldown = attackCooldownDuration;
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);

        if (!IsTargetInRange) yield break;

        IDamageable damageable = _enemy.Target.GetComponent<IDamageable>();
        damageable.TakeDamage(attackDamage);
    }
}