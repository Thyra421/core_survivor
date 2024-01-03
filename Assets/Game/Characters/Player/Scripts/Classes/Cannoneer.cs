using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannoneer : PlayerClass, IRadioactivityUser
{
    [SerializeField]
    private MachineGunShoot machineGunShoot;

    private Player _player;
    private bool _isShooting;

    public Radioactivity Radioactivity { get; } = new();

    private void Awake()
    {
        _player = GetComponent<Player>();
        machineGunShoot.player = _player;
        Abilities = new AbilityBase[] { machineGunShoot };
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled) {
            EndMachineGunCommand();
            _isShooting = false;
            return;
        }

        if (!CanUseAbility(0)) return;

        if (context.started) {
            _isShooting = true;
        }
    }

    [Command]
    private void EndMachineGunCommand()
    {
        machineGunShoot.End();
    }

    protected override void Update()
    {
        base.Update();

        if (!_isShooting) return;

        if (!CanUseAbility(0)) return;

        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);

        if (targetPosition == null) return;

        Vector3 copy = targetPosition.Value;
        copy.y = 0;

        UseAbilityCommand(0, machineGunShoot.Serialize(copy));
    }
}