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
        if (_enemy.IsDead) {
            if (!_navMeshAgent.isStopped)
                _navMeshAgent.isStopped = true;
            return;
        }

        if (_enemy.Target == null) return;

        if (Vector3.Distance(transform.position, _enemy.Target!.position) < _navMeshAgent.stoppingDistance) {
            Vector3 lookDirection = _enemy.Target!.position - transform.position;
            lookDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        _navMeshAgent.SetDestination(_enemy.Target!.position);
    }
}