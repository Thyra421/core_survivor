using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamworks;

public static class SteamworksHelper
{
    /// <summary>
    /// Get the information of a lobby the client is currently in.
    /// </summary>
    public static SteamLobbyInformation GetCurrentLobbyInformation(ulong lobbyId)
    {
        string networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(lobbyId), SteamworksConsts.HostAddressKey);
        string name = SteamMatchmaking.GetLobbyData(new CSteamID(lobbyId), SteamworksConsts.LobbyNameKey);

        return new SteamLobbyInformation(lobbyId, networkAddress, name);
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

    public static async Task<SteamLobbyInformation> GetOtherLobbyInformation(ulong lobbyId)
    {
        bool result = await new DisposableRequestLobbyData().RequestLobbyData(lobbyId);

        if (!result) throw new Exception("RequestLobbyData failed");

        string networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(lobbyId), SteamworksConsts.HostAddressKey);
        string name = SteamMatchmaking.GetLobbyData(new CSteamID(lobbyId), SteamworksConsts.LobbyNameKey);

        return new SteamLobbyInformation(lobbyId, networkAddress, name);
    }

    public static LobbyPlayerInfo GetPlayerInfo(ulong userId)
    {
        string username = SteamFriends.GetFriendPersonaName(new CSteamID(userId));

        return new LobbyPlayerInfo(userId, username);
    }

    public static List<LobbyPlayerInfo> GetPlayersInLobby(ulong lobbyId)
    {
        int membersCount = SteamMatchmaking.GetNumLobbyMembers(new CSteamID(lobbyId));
        List<LobbyPlayerInfo> playerInfos = new();

        for (int i = 0; i < membersCount; i++) {
            CSteamID playerId = SteamMatchmaking.GetLobbyMemberByIndex(new CSteamID(lobbyId), i);

            playerInfos.Add(GetPlayerInfo(playerId.m_SteamID));
        }

        return playerInfos;
    }
}