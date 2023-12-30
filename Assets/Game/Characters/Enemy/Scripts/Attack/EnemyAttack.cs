using Mirror;

public partial class EnemyAttack : NetworkBehaviour
{
    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}