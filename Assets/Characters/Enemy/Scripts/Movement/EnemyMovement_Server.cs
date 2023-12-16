using Mirror;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealth))]
public partial class EnemyMovement
{
    private NavMeshAgent _navMeshAgent;
    private Enemy _enemy;
    private EnemyHealth _enemyHealth;

    public float SpeedRatio => _navMeshAgent.velocity.magnitude;

    public override void OnStartServer()
    {
        base.OnStartServer();
        _enemy = GetComponent<Enemy>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    [Server]
    private void ServerUpdate()
    {
        if (_enemyHealth.IsDead) return;

        if (_enemy.Target == null) return;
        
        _navMeshAgent.SetDestination(_enemy.Target!.position);
    }
}