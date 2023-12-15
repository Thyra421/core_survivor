using Mirror;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
public partial class EnemyMovement
{
    private NavMeshAgent _navMeshAgent;
    private Enemy _enemy;

    public float SpeedRatio => _navMeshAgent.velocity.magnitude;

    public override void OnStartServer()
    {
        base.OnStartServer();
        _enemy = GetComponent<Enemy>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    [Server]
    private void ServerUpdate()
    {
        if (_enemy.Target != null)
            _navMeshAgent.SetDestination(_enemy.Target!.position);
    }
}