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

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
}