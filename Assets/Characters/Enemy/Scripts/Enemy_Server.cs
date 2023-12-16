using JetBrains.Annotations;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public partial class Enemy
{
    private EnemyHealth _enemyHealth;
    [CanBeNull] public Transform Target { get; private set; }

    [Server]
    private void ServerUpdate()
    {
        if (_enemyHealth.IsDead) return;
        FindBestTarget();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    [Server]
    protected virtual void FindBestTarget()
    {
        if (PlayerManager.Current.Players.Count == 0) return;

        Transform target = null;
        float shortestDistance = float.PositiveInfinity;

        foreach (Player p in PlayerManager.Current.Players)
        {
            float distance = Vector3.Distance(transform.position, p.transform.position);

            if (!(distance < shortestDistance)) continue;

            shortestDistance = distance;
            target = p.transform;
        }

        Target = target;
    }
}