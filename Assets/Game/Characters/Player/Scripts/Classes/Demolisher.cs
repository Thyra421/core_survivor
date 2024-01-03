using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isClient || !isOwned || !CanUseAbility(0)) return;

        Vector3? targetPosition = GameHelper.GetMousePositionToWorldPoint(LayerManager.Current.WhatIsGround);

        if (targetPosition == null) return;
        
        Vector3 copy = targetPosition.Value;
        copy.y = 0;

        UseAbilityCommand(0, swordSlash.Serialize(copy));
    }
}