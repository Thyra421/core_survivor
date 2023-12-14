using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public partial class PlayerMovement
{
    private CharacterController _characterController;
    private Vector2 _currentInput;

    [Header("Client")] [SerializeField] private float movementSpeed;

    private void HandleRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        Vector3 direction = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = toRotation;
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new(_currentInput.x, 0, _currentInput.y);

        _characterController.Move(moveDirection * (movementSpeed * Time.deltaTime));
    }

    private void ClientUpdate()
    {
        if (!isOwned) return;

        HandleRotation();
        HandleMovement();
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