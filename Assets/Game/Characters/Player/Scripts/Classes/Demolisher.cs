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

        UseAbilityCommand(0, swordSlash.Serialize(targetPosition.Value));
    }


    public void OnUltimate(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isClient || !isOwned || !CanUseAbility(1)) return;

        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);

        if (targetPosition == null) return;

        UseAbilityCommand(1, blast.Serialize(targetPosition.Value));
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