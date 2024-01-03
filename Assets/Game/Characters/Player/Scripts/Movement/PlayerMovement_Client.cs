using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerMovement
{
    #region Inputs

    [Client]
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (!isClient || !isOwned) return;

        Vector2 input = context.ReadValue<Vector2>();
        MoveCommand(input);
    }

    [Client]
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isClient || !isOwned) return;

        DashCommand();
    }

    #endregion
}