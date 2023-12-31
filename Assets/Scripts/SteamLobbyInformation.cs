public class SteamLobbyInformation
{
    public ulong LobbyId;
    public string NetworkAddress;

    public SteamLobbyInformation(ulong lobbyId, string networkAddress)
    {
        LobbyId = lobbyId;
        NetworkAddress = networkAddress;
    }
}