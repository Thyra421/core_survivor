using Mirror;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(NetworkAnimator))]
public partial class EnemyAnimation
{
    private NetworkAnimator _networkAnimator;
    private Animator _animator;
    private EnemyMovement _enemyMovement;
    private static readonly int SpeedRatioId = Animator.StringToHash("SpeedRatio");

    [Server]
    private void ServerUpdate()
    {
        _animator.SetFloat(SpeedRatioId, _enemyMovement.SpeedRatio);
    }

    [Server]
    public void SetTrigger(string triggerName)
    {
        _networkAnimator.SetTrigger(triggerName);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _animator = GetComponentInChildren<Animator>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _networkAnimator = GetComponent<NetworkAnimator>();
    }
}