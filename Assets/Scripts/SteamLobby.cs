public class SteamLobby
{
    public ulong LobbyId;
    public string NetworkAddress;

    public SteamLobby(ulong lobbyId, string networkAddress)
    {
        LobbyId = lobbyId;
        NetworkAddress = networkAddress;
    }
}