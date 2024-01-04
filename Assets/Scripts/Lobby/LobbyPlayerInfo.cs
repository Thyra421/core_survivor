public class LobbyPlayerInfo
{
    public string Name { get; }

    public ulong Id { get; }

    public Class Class { get; set; } = Class.demolisher;

    public LobbyPlayerInfo(ulong id, string name)
    {
        Name = name;
        Id = id;
    }
}

public enum Class
{
    demolisher,
    cannoneer
}