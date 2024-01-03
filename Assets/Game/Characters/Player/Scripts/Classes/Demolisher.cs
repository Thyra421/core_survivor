using UnityEngine;

[RequireComponent(typeof(Player))]
public class Demolisher : PlayerClass, IRadioactivityUser
{
    [SerializeField]
    private SwordSlash swordSlash;

    private Player _player;

    public Radioactivity Radioactivity { get; } = new();

    private void Awake()
    {
        _player = GetComponent<Player>();
        swordSlash.player = _player;
        Abilities = new AbilityBase[] { swordSlash };
    }

    private void OnAttack()
    {
        if (!isClient || !isOwned || !CanUseAbility(0)) return;

        Vector3 targetPosition = Vector3.zero;
        bool result = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround, ref targetPosition);

        if (!result) return;

        targetPosition.y = 0;

        UseAbilityCommand(0, swordSlash.Serialize(targetPosition));
    }
}