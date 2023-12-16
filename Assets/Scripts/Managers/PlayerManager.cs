public class PlayerManager : Singleton<PlayerManager>
{
    public Listenable<Player> LocalPlayer { get; } = new();

    public ListenableList<Player> Players { get; } = new();
}