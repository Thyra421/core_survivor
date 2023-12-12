using Mirror;

public partial class PlayerMovement : NetworkBehaviour
{
    private void Update()
    {
        if (isClient)
            ClientUpdate();
    }
}