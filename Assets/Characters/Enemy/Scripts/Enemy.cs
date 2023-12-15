using Mirror;

public partial class Enemy : NetworkBehaviour
{
    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}