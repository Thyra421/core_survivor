using Steamworks;

public static class SteamworksHelper
{
    public static SteamLobbyInformation GetLobbyInformation(ulong id)
    {
        string networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.HostAddressKey);
        string name = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.LobbyNameKey);

        return new SteamLobbyInformation(id, networkAddress, name);
    }
}