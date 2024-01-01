using System;
using System.Threading.Tasks;
using Steamworks;

public class SteamworksHelper
{
    /// <summary>
    /// Get the information of a lobby the client is currently in.
    /// </summary>
    public static SteamLobbyInformation GetCurrentLobbyInformation(ulong id)
    {
        string networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.HostAddressKey);
        string name = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.LobbyNameKey);

        return new SteamLobbyInformation(id, networkAddress, name);
    }

    public async Task<SteamLobbyInformation> GetOtherLobbyInformation(ulong id)
    {
        TaskCompletionSource<bool> taskCompletionSource = new();

        Callback<LobbyDataUpdate_t>.Create(callback =>
            taskCompletionSource.SetResult(callback.m_bSuccess == 1));

        SteamMatchmaking.RequestLobbyData(new CSteamID(id));

        bool result = await taskCompletionSource.Task;

        if (!result) throw new Exception("RequestLobbyData failed");

        string networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.HostAddressKey);
        string name = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.LobbyNameKey);

        return new SteamLobbyInformation(id, networkAddress, name);
    }
}