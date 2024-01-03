using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannoneer : PlayerClass, IRadioactivityUser
{
    [SerializeField]
    private MachineGunShoot machineGunShoot;

    private Player _player;
    private bool _isShooting;
    private Vector3? _target;

    public Radioactivity Radioactivity { get; } = new();
    public override Vector3? Target => _target ?? base.Target;

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
            _target = null;
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
        machineGunShoot.ServerEnd();
    }

    protected override void Update()
    {
        base.Update();

        if (!_isShooting) return;
        
        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);

        if (targetPosition == null) return;

        Vector3 copy = targetPosition.Value;
        copy.y = 0;
        _target = copy;

        if (!CanUseAbility(0)) return;

        UseAbilityCommand(0, machineGunShoot.Serialize(copy));
    }
}