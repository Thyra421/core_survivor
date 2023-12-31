public class SteamLobbyInformation
{
    public readonly ulong id;
    public readonly string networkAddress;
    public readonly string name;

    public SteamLobbyInformation(ulong id, string networkAddress, string name)
    {
        this.id = id;
        this.networkAddress = networkAddress;
        this.name = name;
    }
}