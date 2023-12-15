using Mirror;

public partial class EnemyAnimation : NetworkBehaviour
{
    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}