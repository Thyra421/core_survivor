using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public partial class PlayerAttack
{
    [SerializeField] private LayerMask whatIsGround;
    private PlayerAnimation _playerAnimation;

    [ClientRpc]
    private void ClientAttack()
    {
        _playerAnimation.SetTrigger("Attack");

        if (!isOwned) return;

        _cooldown = cooldownDuration;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void OnAttack()
    {
        if (!isClient || !isOwned) return;

        if (_cooldown > 0) return;

        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main!.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100, whatIsGround)) return;

        Vector3 targetPosition = hit.point;
        targetPosition.y = 0;

        AttackCommand(targetPosition);
    }
}