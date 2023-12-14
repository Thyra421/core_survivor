using Mirror;

public partial class Player : NetworkBehaviour
{
    private void Awake()
    {
        PlayerManager.Current.Players.Add(this);
    }

    private void OnDestroy()
    {
        PlayerManager.Current.Players.Remove(this);
    }
}
