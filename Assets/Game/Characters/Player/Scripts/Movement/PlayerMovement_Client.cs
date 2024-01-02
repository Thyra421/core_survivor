using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerMovement
{
    #region Inputs

    [Client]
    public void OnMovement(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        MoveCommand(input);
    }

    [Client]
    public void OnDash()
    {
        if (!isClient || !isOwned) return;
        DashCommand();
    }

    #endregion
}