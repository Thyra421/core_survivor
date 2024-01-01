using System;
using System.Threading.Tasks;
using Steamworks;

public static class SteamworksHelper
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

    private class DisposableRequestLobbyData
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();
        private Callback<LobbyDataUpdate_t> _lobbyDataUpdateCallback;

        public DisposableRequestLobbyData()
        {
            _lobbyDataUpdateCallback = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
        }

        private void OnLobbyDataUpdate(LobbyDataUpdate_t callback)
        {
            if (_taskCompletionSource != null && !_taskCompletionSource.Task.IsCompleted)
                _taskCompletionSource.SetResult(callback.m_bSuccess == 1);
        }

        public Task<bool> RequestLobbyData(ulong id)
        {
            SteamMatchmaking.RequestLobbyData(new CSteamID(id));

            return _taskCompletionSource.Task;
        }
    }

    public static async Task<SteamLobbyInformation> GetOtherLobbyInformation(ulong id)
    {
        bool result = await new DisposableRequestLobbyData().RequestLobbyData(id);

        if (!result) throw new Exception("RequestLobbyData failed");

        string networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.HostAddressKey);
        string name = SteamMatchmaking.GetLobbyData(new CSteamID(id), SteamworksConsts.LobbyNameKey);

        return new SteamLobbyInformation(id, networkAddress, name);
    }
}