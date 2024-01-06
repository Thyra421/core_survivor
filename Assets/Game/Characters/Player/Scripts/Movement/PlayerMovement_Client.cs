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
        if (_player.IsDead) return;

        Vector2 input = context.ReadValue<Vector2>();
        MoveCommand(input);
    }

    [Client]
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isClient || !isOwned) return;
        if (_player.IsDead) return;

        Vector3? target = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);
        if (target == null) return;
        Vector3 direction = target.Value - transform.position;
        DashCommand(direction);
    }

    #endregion
}