using JetBrains.Annotations;
using Mirror;
using UnityEngine;

public partial class Enemy
{
    [CanBeNull] public Transform Target { get; private set; }

    [Server]
    private void ServerUpdate()
    {
        FindBestTarget();
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