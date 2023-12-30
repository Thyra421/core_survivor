using Mirror;

public partial class PlayerAnimation : NetworkBehaviour
{
    private void Update()
    {
        if (isClient)
            ClientUpdate();
    }
}