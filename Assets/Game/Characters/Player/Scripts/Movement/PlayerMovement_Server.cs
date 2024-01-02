﻿using System.Collections;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public partial class PlayerMovement
{
    [Header("Speed")] [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [Header("Dash")] [SerializeField] private float dashDistance;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldownDuration;
    [SerializeField] private float dashStaminaCostPerSecond;
    [SerializeField] private float staminaRegenerationPerSecond;

    private CharacterController _characterController;
    private Vector2 _currentInput;
    private float _currentSpeed;
    private Vector3 _momentum;
    private bool _canMove = true;
    private float _dashCooldown;

    public readonly Listenable<float> stamina = new(100);

    public float SpeedRatio => _currentSpeed == 0 ? 0 : _currentSpeed / movementSpeed;

    #region Movement

    [Command]
    private void MoveCommand(Vector2 input)
    {
        _currentInput = input;
    }

    [Server]
    private void CalculateMomentum()
    {
        Vector3 moveDirection = new(_currentInput.x, 0, _currentInput.y);

        if (moveDirection.magnitude > 0) {
            if (Vector3.Dot(_momentum, moveDirection) < 0)
                _currentSpeed = 0;
            _currentSpeed = Mathf.Clamp(_currentSpeed + acceleration * Time.deltaTime, 0, movementSpeed);
            _momentum = moveDirection * (_currentSpeed * Time.deltaTime);
        }
        else {
            _currentSpeed = Mathf.Clamp(_currentSpeed - deceleration * Time.deltaTime, 0, movementSpeed);
            _momentum = _momentum.normalized * (_currentSpeed * Time.deltaTime);
            if (_currentSpeed == 0) _momentum = Vector3.zero;
        }
    }

    [Server]
    private void HandleMovementAndRotation()
    {
        CalculateMomentum();

        if (_momentum.magnitude == 0) return;

        Quaternion lookRotation = Quaternion.LookRotation(_momentum);

        _characterController.Move(_momentum);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Dash

    [Command]
    private void DashCommand()
    {
        if (_dashCooldown > 0) return;

        Dash();
    }

    [Server]
    private void Dash()
    {
        _dashCooldown = dashCooldownDuration;
        _canMove = false;
        _currentSpeed = dashSpeed;
        StartCoroutine(nameof(DashCoroutine));
    }

    [Server]
    private void OnFinishDash()
    {
        _canMove = true;
    }

    [Server]
    private IEnumerator DashCoroutine()
    {
        Vector3 originalPosition = transform.position;
        float timeout = dashDistance / dashSpeed;

        while (Vector3.Distance(transform.position, originalPosition) < dashDistance && timeout > 0 &&
               stamina.Value > 0) {
            _characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
            stamina.Value = Mathf.Clamp(stamina.Value - dashStaminaCostPerSecond * Time.deltaTime, 0, 100);
            timeout -= Time.deltaTime;
            yield return null;
        }

        OnFinishDash();
        yield return null;
    }

    #endregion

    [Server]
    private void ServerUpdate()
    {
        if (!_canMove) return;

        HandleMovementAndRotation();
        _dashCooldown -= Time.deltaTime;
        stamina.Value = Mathf.Clamp(stamina.Value + staminaRegenerationPerSecond * Time.deltaTime, 0, 100);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _characterController = GetComponent<CharacterController>();
    }
}