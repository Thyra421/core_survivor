using System;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
public partial class EnemyMovement : NetworkBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}