using Mirror;

public partial class PlayerMovement : NetworkBehaviour
{
    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
}