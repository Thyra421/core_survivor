using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannoneer : PlayerClass
{
    [SerializeField]
    private MachineGunShoot machineGunShoot;

    [SerializeField]
    private Flamethrower flamethrower;

    [SerializeField]
    private Transform aim;

    private Player _player;

    [Client]
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!(isClient && isOwned)) return;

        if (context.canceled) {
            StopUsingAbilityCommand(0);
            return;
        }

        if (!CanUseAbility(0)) return;

        if (context.started) {
            StartUsingAbilityCommand(0);
        }
    }

    [Client]
    public void OnUltimate(InputAction.CallbackContext context)
    {
        if (!(isClient && isOwned)) return;

        if (context.canceled) {
            StopUsingAbilityCommand(1);
            return;
        }

        if (!CanUseAbility(1)) return;

        if (context.started) {
            StartUsingAbilityCommand(1);
        }
    }

    #region Aim

    [Client]
    private void Aim()
    {
        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);
        if (targetPosition == null) return;
        Vector3 aimPosition = targetPosition.Value;
        aimPosition.y = 1.5f;
        SetAimCommand(aimPosition);
        SetTargetCommand(targetPosition.Value);
    }

    [Command]
    private void SetAimCommand(Vector3 aimPosition)
    {
        aim.position = aimPosition;
        SetAimRpc(aimPosition);
    }

    [ClientRpc]
    private void SetAimRpc(Vector3 aimPosition)
    {
        aim.position = aimPosition;
    }

    #endregion

    protected override void Update()
    {
        base.Update();

        if (!(isClient && isOwned)) return;

        if (flamethrower.IsUsing || machineGunShoot.IsUsing)
            Aim();
    }

    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<Player>();
        machineGunShoot.player = _player;
        flamethrower.player = _player;
        Abilities = new AbilityBase[] { machineGunShoot, flamethrower };
    }
}