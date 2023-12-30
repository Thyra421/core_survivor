using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkAnimator))]
[RequireComponent(typeof(EnemyMovement))]
public partial class EnemyAnimation
{
    private EnemyMovement _enemyMovement;
    private NetworkAnimator _networkAnimator;
    private static readonly int SpeedRatioId = Animator.StringToHash("SpeedRatio");

    [Server]
    private void ServerUpdate()
    {
        _animator.SetFloat(SpeedRatioId, _enemyMovement.SpeedRatio);
    }

    [Server]
    public void ServerSetTrigger(string triggerName)
    {
        _networkAnimator.SetTrigger(triggerName);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _enemyMovement = GetComponent<EnemyMovement>();
        _networkAnimator = GetComponent<NetworkAnimator>();
    }
}