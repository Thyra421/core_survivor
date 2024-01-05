using UnityEngine;
using UnityEngine.InputSystem;

public class Demolisher : PlayerClass
{
    [SerializeField]
    private SwordSlash swordSlash;

    [SerializeField]
    private Blast blast;

    private Player _player;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isClient || !isOwned || !CanUseAbility(0)) return;

        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);

        if (targetPosition == null) return;

        SetTargetCommand(targetPosition!.Value);
        UseAbilityCommand(0);
    }

    public void OnUltimate(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isClient || !isOwned || !CanUseAbility(1)) return;

        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);

        if (targetPosition == null) return;

        SetTargetCommand(targetPosition!.Value);
        UseAbilityCommand(1);
    }

    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<Player>();
        swordSlash.player = _player;
        blast.player = _player;
        Abilities = new AbilityBase[] { swordSlash, blast };
    }
}