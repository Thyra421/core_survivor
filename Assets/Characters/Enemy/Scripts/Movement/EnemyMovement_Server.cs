using Mirror;

public partial class EnemyMovement
{
    [Server]
    private void ServerUpdate()
    {
        if (_enemy.Target != null)
            _navMeshAgent.SetDestination(_enemy.Target!.position);
    }
}