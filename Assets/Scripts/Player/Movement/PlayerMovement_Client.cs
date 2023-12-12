using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public partial class PlayerMovement
{
    private CharacterController _characterController;
    private Vector2 _currentInput;

    [Header("Client")] [SerializeField] private float movementSpeed;

    private void ClientUpdate()
    {
        if (!isOwned) return;

        Vector3 moveDirection = new(_currentInput.x, 0, _currentInput.y);
        _characterController.Move(moveDirection * (movementSpeed * Time.deltaTime));
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