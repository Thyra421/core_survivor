using Mirror;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public partial class EnemyAnimation
{
    private EnemyMovement _enemyMovement;
    private static readonly int SpeedRatioId = Animator.StringToHash("SpeedRatio");

    [Server]
    private void ServerUpdate()
    {
        _animator.SetFloat(SpeedRatioId, _enemyMovement.SpeedRatio);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _enemyMovement = GetComponent<EnemyMovement>();
    }
}