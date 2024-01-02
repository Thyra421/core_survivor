﻿using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public partial class PlayerAnimation
{
    private PlayerMovement _playerMovement;
    private static readonly int MovementXId = Animator.StringToHash("MovementX");
    private static readonly int MovementYId = Animator.StringToHash("MovementY");
    private static readonly int SpeedRatioId = Animator.StringToHash("SpeedRatio");

    [Server]
    private void ServerUpdate()
    {
        Vector3 moveDirection = _playerMovement.MoveDirection.normalized;
        Vector3 lookDirection = _playerMovement.LookDirection.normalized;

        Vector2 animationDirection = ReframeVector(moveDirection, lookDirection);

        _animator.SetFloat(MovementXId, animationDirection.y);
        _animator.SetFloat(MovementYId, animationDirection.x);
        _animator.SetFloat(SpeedRatioId, _playerMovement.SpeedRatio);
    }

    private static Vector2 ReframeVector(Vector3 a, Vector3 b)
    {
        float denominator = Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);

        float cX = (a.x * b.x + a.y * b.y + a.z * b.z) / denominator;
        float cZ = (a.x * b.z - a.z * b.x) / denominator;

        return new Vector2(cX, cZ);
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
        _playerMovement = GetComponent<PlayerMovement>();
    }
}