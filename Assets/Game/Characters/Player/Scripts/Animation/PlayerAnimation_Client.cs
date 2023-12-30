using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public partial class PlayerAnimation
{
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private static readonly int SpeedRatioId = Animator.StringToHash("SpeedRatio");

    [Client]
    private void ClientUpdate()
    {
        if (isOwned)
            _animator.SetFloat(SpeedRatioId, _playerMovement.SpeedRatio);
    }

    [Client]
    public void SetTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    public override void OnStartClient()
    {
        base.OnStartLocalPlayer();
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
}