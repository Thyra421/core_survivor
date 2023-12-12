using Mirror;

public partial class PlayerAttack
{
    [ClientRpc]
    private void Attack()
    {
        Log.Client("Attack");
    }

    public void OnAttack()
    {
        if (isClient && isOwned)
            AttackCommand();
    }
}