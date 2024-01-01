public class LobbyPlayerInfo
{
    public string Name { get; }

    public ulong Id { get; }

    public LobbyPlayerInfo(ulong id, string name)
    {
        Name = name;
        Id = id;
    }
}