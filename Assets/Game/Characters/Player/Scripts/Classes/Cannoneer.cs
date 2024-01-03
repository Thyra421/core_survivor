using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Cannoneer : PlayerClass, IRadioactivityUser
{
    [SerializeField]
    private MachineGunShoot machineGunShoot;

    [SerializeField]
    private Flamethrower flamethrower;

    [SerializeField]
    private Transform aim;

    [SerializeField]
    private Rig rig;

    private Player _player;
    private bool _isShooting;
    private bool _isFlaming;
    private Vector3? _target;

    public Radioactivity Radioactivity { get; } = new();
    public override Vector3? Target => _target ?? base.Target;

    private void Awake()
    {
        _player = GetComponent<Player>();
        machineGunShoot.player = _player;
        flamethrower.player = _player;
        Abilities = new AbilityBase[] { machineGunShoot, flamethrower };
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!(isClient && isOwned)) return;

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

    public void OnUltimate(InputAction.CallbackContext context)
    {
        if (!(isClient && isOwned)) return;

        if (context.canceled) {
            EndFlamethrowerCommand();
            _isFlaming = false;
            _target = null;
            rig.weight = 0;

            return;
        }

        if (!CanUseAbility(1)) return;

        if (context.started) {
            _isFlaming = true;
            rig.weight = 1;
        }
    }

    [Command]
    private void EndMachineGunCommand()
    {
        machineGunShoot.ServerEnd();
    }

    [Command]
    private void EndFlamethrowerCommand()
    {
        flamethrower.ServerEnd();
        ClientEnd();
    }

    [ClientRpc]
    private void ClientEnd()
    {
        flamethrower.ClientEnd();
    }

    private void ShootingUpdate()
    {
        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);
        if (targetPosition == null) return;
        _target = targetPosition.Value;
        Vector3 aimPosition = _target.Value;
        aimPosition.y = 1.5f;
        aim.transform.position = aimPosition;

        if (!CanUseAbility(0)) return;

        UseAbilityCommand(0, machineGunShoot.Serialize(_target.Value));
    }

    private void FlamethrowerUpdate()
    {
        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);
        if (targetPosition == null) return;
        _target = targetPosition.Value;
        Vector3 aimPosition = _target.Value;
        aimPosition.y = 1.5f;
        aim.transform.position = aimPosition;

        if (!CanUseAbility(1)) return;

        UseAbilityCommand(1, flamethrower.Serialize(_target.Value));
    }

    protected override void Update()
    {
        base.Update();

        if (!(isClient && isOwned)) return;

        if (_isShooting) ShootingUpdate();
        if (_isFlaming) FlamethrowerUpdate();
    }
}