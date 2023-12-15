using Mirror;

public partial class PlayerAttack : NetworkBehaviour
{
    [Command]
    private void AttackCommand()
    {
        ClientAttack();
        ServerAttack();
    }
}
