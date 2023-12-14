using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{
    public List<Player> Players { get; private set; } = new();
}