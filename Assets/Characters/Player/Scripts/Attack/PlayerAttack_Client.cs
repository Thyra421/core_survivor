using Mirror;

public partial class PlayerAttack
{
    [ClientRpc]
    private void ClientAttack()
    {
        ConsoleLogger.Client("Attack");
    }

    public void OnAttack()
    {
        if (isClient && isOwned)
            AttackCommand();
    }
}