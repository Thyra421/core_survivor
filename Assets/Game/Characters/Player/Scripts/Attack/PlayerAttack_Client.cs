using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public partial class PlayerAttack
{
    [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField]
    private Transform handTransform;

    [SerializeField]
    private LayerMask whatIsGround;

    private PlayerAnimation _playerAnimation;

    [ClientRpc]
    private void ClientAttack()
    {
        _playerAnimation.SetTrigger("Attack");

        if (!isOwned) return;

        Cooldown.Start();
    }

    [ClientRpc]
    private void ClientThrow()
    {
        _playerAnimation.SetTrigger("Throw");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void OnAttack()
    {
        if (!isClient || !isOwned) return;
        if (!Cooldown.IsReady) return;

        Vector3 targetPosition = Vector3.zero;
        bool result = GameHelper.GetMousePositionToWorldPoint(whatIsGround, ref targetPosition);

        if (!result) return;

        targetPosition.y = 0;
        AttackCommand(targetPosition);
    }

    public void OnUltimate()
    {
        Grenade grenade = Instantiate(grenadePrefab, handTransform.position, Quaternion.identity)
            .GetComponent<Grenade>();

        Vector3 targetPosition = Vector3.zero;
        bool result = GameHelper.GetMousePositionToWorldPoint(whatIsGround, ref targetPosition);

        if (!result) return;

        grenade.Initialize(targetPosition);
    }
}