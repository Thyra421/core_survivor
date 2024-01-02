using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public partial class PlayerAnimation
{
    private PlayerMovement _playerMovement;
    private static readonly int SpeedRatioId = Animator.StringToHash("SpeedRatio");

    [Server]
    private void ServerUpdate()
    {
        _animator.SetFloat(SpeedRatioId, _playerMovement.SpeedRatio);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    
}