using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Cannoneer : PlayerClass, IRadioactivityUser
{
    [SerializeField]
    private MachineGunShoot machineGunShoot;

    [SerializeField]
    private Transform aim;

    [SerializeField]
    private Rig rig;

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
            rig.weight = 0;

            return;
        }

        if (!CanUseAbility(0)) return;

        if (context.started) {
            _isShooting = true;
            rig.weight = 1;
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
        _target = targetPosition.Value;
        Vector3 aimPosition = _target.Value;
        aimPosition.y = 1.5f;
        aim.transform.position = aimPosition;

        if (!CanUseAbility(0)) return;

        UseAbilityCommand(0, machineGunShoot.Serialize(_target.Value));
    }
}