using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealth))]
public partial class Enemy
{
    [SerializeField]
    private EnemyTargetPreference targetPreference;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    private EnemyHealth _enemyHealth;
    [CanBeNull] public Transform Target { get; private set; }

    public bool IsDead => _enemyHealth.IsDead;

    [Server]
    private void ServerUpdate()
    {
        if (IsDead) return;

        FindBestTarget();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    [Server]
    private void FindBestTarget()
    {
        Default();
        
        // bool hasFoundTarget;
        //
        // switch (targetPreference) {
        //     case EnemyTargetPreference.core:
        //         hasFoundTarget = FindCore() || FindClosest();
        //         break;
        //     case EnemyTargetPreference.player:
        //         hasFoundTarget = FindBestPlayer() || FindClosest();
        //         break;
        //     case EnemyTargetPreference.environment:
        //         hasFoundTarget = FindBestEnvironment() || FindClosest();
        //         break;
        //     case EnemyTargetPreference.closest:
        //         hasFoundTarget = FindClosest();
        //         break;
        // }
    }

    [Server]
    private bool FindCore()
    {
        NavMeshPath path = new();

        if (EnvironmentManager.Current.Core == null) return false;

        if (!navMeshAgent.CalculatePath(EnvironmentManager.Current.Core.position, path))
            return false;

        Target = EnvironmentManager.Current.Core;
        return true;
    }

    [Server]
    private bool FindBestEnvironment()
    {
        if (EnvironmentManager.Current.Environments.Count == 0) return false;

        Transform target = null;
        float shortestDistance = float.PositiveInfinity;

        foreach (DestructibleEnvironment e in EnvironmentManager.Current.Environments) {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            NavMeshPath path = new();

            if (distance > shortestDistance || !navMeshAgent.CalculatePath(e.transform.position, path)) continue;

            shortestDistance = distance;
            target = e.transform;
        }

        if (target == null) return false;

        Target = target;
        return true;
    }

    [Server]
    private bool FindBestPlayer()
    {
        if (PlayerManager.Current.Players.Count == 0) return false;

        Transform target = null;
        float shortestDistance = float.PositiveInfinity;

        foreach (Player p in PlayerManager.Current.Players) {
            float distance = Vector3.Distance(transform.position, p.transform.position);
            NavMeshPath path = new();

            if (distance > shortestDistance || !navMeshAgent.CalculatePath(p.transform.position, path)) continue;

            shortestDistance = distance;
            target = p.transform;
        }

        if (target == null) return false;

        Target = target;
        return true;
    }

    [Server]
    private bool FindClosest()
    {
        if (PlayerManager.Current.Players.Count == 0) return false;

        Transform target = null;
        float shortestDistance = float.PositiveInfinity;

        foreach (DestructibleEnvironment e in EnvironmentManager.Current.Environments) {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            NavMeshPath path = new();

            if (distance > shortestDistance || !navMeshAgent.CalculatePath(e.transform.position, path)) continue;

            shortestDistance = distance;
            target = e.transform;
        }

        foreach (Player p in PlayerManager.Current.Players) {
            float distance = Vector3.Distance(transform.position, p.transform.position);
            NavMeshPath path = new();

            if (distance > shortestDistance || !navMeshAgent.CalculatePath(p.transform.position, path)) continue;

            shortestDistance = distance;
            target = p.transform;
        }

        if (target == null)
            return false;

        Target = target;
        return true;
    }

    [Server]
    private bool Default()
    {
        if (PlayerManager.Current.Players.Count == 0) return false;

        Transform target = null;
        float shortestDistance = float.PositiveInfinity;

        foreach (DestructibleEnvironment e in EnvironmentManager.Current.Environments) {
            float distance = Vector3.Distance(transform.position, e.transform.position);

            if (distance > shortestDistance) continue;

            shortestDistance = distance;
            target = e.transform;
        }

        foreach (Player p in PlayerManager.Current.Players) {
            float distance = Vector3.Distance(transform.position, p.transform.position);

            if (distance > shortestDistance || p.IsDead) continue;

            shortestDistance = distance;
            target = p.transform;
        }

        if (target == null)
            return false;

        Target = target;
        return true;
    }
}

public enum EnemyTargetPreference
{
    closest,
    player,
    environment,
    core
}