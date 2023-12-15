using Mirror;

public partial class EnemyMovement : NetworkBehaviour
{
    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}