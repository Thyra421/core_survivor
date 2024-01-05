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
    private bool _isShooting;
    private bool _isFlaming;

    private Vector3? _target;

    public override Vector3? Target => _target ?? base.Target;

    private void Awake()
    {
        _player = GetComponent<Player>();
        machineGunShoot.player = _player;
        flamethrower.player = _player;
        Abilities = new AbilityBase[] { machineGunShoot, flamethrower };
    }

    [Client]
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!(isClient && isOwned)) return;

        if (context.canceled) {
            EndUseAbilityCommand(0, "");
            _isShooting = false;
            ResetTargetCommand();

            return;
        }

        if (!CanUseAbility(0)) return;

        if (context.started) {
            _isShooting = true;
        }
    }

    [Client]
    public void OnUltimate(InputAction.CallbackContext context)
    {
        if (!(isClient && isOwned)) return;

        if (context.canceled) {
            EndUseAbilityCommand(1, "");
            _isFlaming = false;
            ResetTargetCommand();

            return;
        }

        if (!CanUseAbility(1)) return;

        if (context.started) {
            _isFlaming = true;
        }
    }

    [Command]
    private void SetAimAndTargetCommand(Vector3 aimPosition, Vector3 targetPosition)
    {
        aim.position = aimPosition;
        SetAimRpc(aimPosition, targetPosition);
    }

    [ClientRpc]
    private void SetAimRpc(Vector3 aimPosition, Vector3 targetPosition)
    {
        aim.position = aimPosition;
        _target = targetPosition;
    }

    [Command]
    private void ResetTargetCommand()
    {
        _target = null;
        ResetTargetRpc();
    }
    
    [ClientRpc]
    private void ResetTargetRpc()
    {
        _target = null;
    }

    [Client]
    private void ShootingUpdate()
    {
        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);
        if (targetPosition == null) return;
        Vector3 aimPosition = targetPosition.Value;
        aimPosition.y = 1.5f;
        SetAimAndTargetCommand(aimPosition, targetPosition.Value);

        if (!CanUseAbility(0)) return;

        UseAbilityCommand(0, machineGunShoot.Serialize(targetPosition.Value));
    }

    [Client]
    private void FlamethrowerUpdate()
    {
        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);
        if (targetPosition == null) return;
        Vector3 aimPosition = targetPosition.Value;
        aimPosition.y = 1.5f;
        SetAimAndTargetCommand(aimPosition, targetPosition.Value);

        if (!CanUseAbility(1)) return;

        UseAbilityCommand(1, flamethrower.Serialize(targetPosition.Value));
    }

    protected override void Update()
    {
        base.Update();

        if (!(isClient && isOwned)) return;

        if (_isShooting) ShootingUpdate();
        if (_isFlaming) FlamethrowerUpdate();
    }
}