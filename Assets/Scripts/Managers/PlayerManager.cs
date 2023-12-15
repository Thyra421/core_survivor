public class PlayerManager : Singleton<PlayerManager>
{
    public Listenable<Player> LocalPlayer { get; set; } = new();

    public ListenableList<Player> Players { get; private set; } = new();
}