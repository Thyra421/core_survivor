using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public partial class PlayerAnimation
{
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private static readonly int SpeedRatioId = Animator.StringToHash("SpeedRatio");

    [Server]
    private void ServerUpdate()
    {
        _animator.SetFloat(SpeedRatioId, _playerMovement.SpeedRatio);
    }

    [Server]
    public void SetTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    public override void OnStartServer()
    {
        base.OnStartLocalPlayer();
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
}