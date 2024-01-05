using System.Collections;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public partial class PlayerMovement
{
    [Header("Speed")]
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private float deceleration;

    [Header("Dash")]
    [SerializeField]
    private float dashDistance;

    [SerializeField]
    private float dashSpeed;

    [SerializeField]
    private float dashStaminaCostPerSecond;

    [SerializeField]
    private float staminaRegenerationPerSecond;

    private PlayerClass _playerClass;
    private CharacterController _characterController;
    private Vector2 _currentInput;
    private float _currentSpeed;
    private Vector3 _momentum;
    private Vector3 _moveDirection;
    private bool _canMove = true;

    public readonly Listenable<float> stamina = new(100);

    public float SpeedRatio => _currentSpeed == 0 ? 0 : _currentSpeed / movementSpeed;

    public Vector3 MoveDirection => _moveDirection;

    public Vector3 LookDirection =>
        _playerClass.Target != null ? _playerClass.Target.Value - transform.position : MoveDirection;

    #region Movement

    [Command]
    private void MoveCommand(Vector2 input)
    {
        _currentInput = input;
    }

    [Server]
    private void CalculateMomentum()
    {
        _moveDirection = new Vector3(_currentInput.x, 0, _currentInput.y);

        // if the player is currently pressing movement keys
        if (_moveDirection.magnitude > 0) {
            // if the player is going to the opposite side they were going
            if (Vector3.Dot(_momentum, _moveDirection) < 0)
                _currentSpeed = 0;
            _currentSpeed = Mathf.Clamp(_currentSpeed + acceleration * Time.deltaTime, 0, movementSpeed);
            _momentum = _moveDirection * (_currentSpeed * Time.deltaTime);
        }
        // player is not pressing movement keys. decrease movement progressively
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

        if (_momentum.magnitude > 0) {
            _characterController.Move(_momentum);
        }

        if (LookDirection.magnitude <= 0) return;

        Vector3 lookDirection = LookDirection.normalized;
        lookDirection.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Dash

    [Command]
    private void DashCommand(Vector3 direction)
    {
        if (!dashCooldown.IsReady || _playerClass.IsBusy) return;

        Dash(direction);
    }

    [Server]
    private void Dash(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);

        dashCooldown.Start();
        _canMove = false;
        _currentSpeed = dashSpeed;
        StartCoroutine(nameof(DashCoroutine));
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

        _canMove = true;
        yield return null;
    }

    #endregion 

    [Server]
    private void ServerUpdate()
    {
        if (!_canMove) return;

        HandleMovementAndRotation();
        dashCooldown.Update();
        stamina.Value = Mathf.Clamp(stamina.Value + staminaRegenerationPerSecond * Time.deltaTime, 0, 100);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _characterController = GetComponent<CharacterController>();
        _playerClass = GetComponent<PlayerClass>();
    }
}