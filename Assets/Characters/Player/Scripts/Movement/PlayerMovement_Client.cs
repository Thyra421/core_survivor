using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public partial class PlayerMovement
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    private CharacterController _characterController;
    private Vector2 _currentInput;
    private float _currentSpeed;
    private Vector3 _momentum;

    public float SpeedRatio { get; private set; }

    [Client]
    private void CalculateMomentum()
    {
        Vector3 moveDirection = new(_currentInput.x, 0, _currentInput.y);

        if (moveDirection.magnitude > 0)
        {
            if (Vector3.Dot(_momentum, moveDirection) < 0)
                _currentSpeed = 0;
            _currentSpeed = Mathf.Clamp(_currentSpeed + acceleration * Time.deltaTime, 0, movementSpeed);
            _momentum = moveDirection * (_currentSpeed * Time.deltaTime);
        }
        else
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed - deceleration * Time.deltaTime, 0, movementSpeed);
            _momentum = _momentum.normalized * (_currentSpeed * Time.deltaTime);
            if (_currentSpeed == 0) _momentum = Vector3.zero;
        }
    }

    [Client]
    private void HandleMovementAndRotation()
    {
        CalculateMomentum();

        if (_momentum.magnitude == 0) return;

        Quaternion lookRotation = Quaternion.LookRotation(_momentum);

        _characterController.Move(_momentum);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    [Client]
    private void ClientUpdate()
    {
        if (!isOwned) return;

        HandleMovementAndRotation();
        SpeedRatio = _currentSpeed == 0 ? 0 : _currentSpeed / movementSpeed;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        _characterController = GetComponent<CharacterController>();
    }

    public void OnMovement(InputValue value)
    {
        if (!isClient || !isOwned) return;

        _currentInput = value.Get<Vector2>();
    }
}