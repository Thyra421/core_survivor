using Steamworks;

public static class SteamworksHelper
{
    private static readonly Callback<LobbyDataUpdate_t> lobbyDataUpdate;

    static SteamworksHelper()
    {
        lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
    }

    /// <summary>
    /// Get the information of a lobby the client is currently in.
    /// </summary>
    public static SteamLobbyInformation GetCurrentLobbyInformation(ulong id)
    {
        string networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.HostAddressKey);
        string name = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.LobbyNameKey);

        return new SteamLobbyInformation(id, networkAddress, name);
    }

    public static SteamLobbyInformation GetOtherLobbyInformation(ulong id)
    {
        SteamMatchmaking.RequestLobbyData(new CSteamID(id));

        // string networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.HostAddressKey);
        // string name = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.LobbyNameKey);
        //
        // return new SteamLobbyInformation(id, networkAddress, name);
        return null;
    }

    private static void OnLobbyDataUpdate(LobbyDataUpdate_t callback)
    {
        ConsoleLogger.Steamworks(callback.m_bSuccess);

        string networkAddress =
            SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), SteamworksConsts.HostAddressKey);
        string name =
            SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), SteamworksConsts.LobbyNameKey);

        ConsoleLogger.Steamworks($"{networkAddress}   {name}");
    }
}