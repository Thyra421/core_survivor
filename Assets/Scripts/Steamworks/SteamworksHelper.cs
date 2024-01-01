using System;
using System.Threading.Tasks;
using Steamworks;

public class SteamworksHelper
{
    protected readonly Callback<LobbyDataUpdate_t> lobbyDataUpdate;
    private TaskCompletionSource<bool> _taskCompletionSource;

    public SteamworksHelper()
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

    public async Task<SteamLobbyInformation> GetOtherLobbyInformation(ulong id)
    {
        if (_taskCompletionSource != null && !_taskCompletionSource.Task.IsCompleted) {
            throw new Exception("Previous RequestLobbyData request is not completed.");
        }

        _taskCompletionSource = new TaskCompletionSource<bool>();

        SteamMatchmaking.RequestLobbyData(new CSteamID(id));

        bool result = await _taskCompletionSource.Task;

        if (!result) throw new Exception("RequestLobbyData failed");

        string networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.HostAddressKey);
        string name = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.LobbyNameKey);

        return new SteamLobbyInformation(id, networkAddress, name);
    }

    private void OnLobbyDataUpdate(LobbyDataUpdate_t callback)
    {
        ConsoleLogger.Steamworks(callback.m_ulSteamIDLobby);
        _taskCompletionSource.SetResult(callback.m_bSuccess == 1);
    }
}