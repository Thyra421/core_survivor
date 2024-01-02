using System;
using System.Threading.Tasks;
using Steamworks;

public class SteamworksLobbyAPI
{
    // Assign callbacks to local variables to avoid garbage collection.
    private readonly Callback<LobbyCreated_t> _lobbyCreated;
    private readonly Callback<GameLobbyJoinRequested_t> _joinRequested;
    private readonly Callback<LobbyEnter_t> _lobbyEnter;
    private readonly Callback<LobbyChatUpdate_t> _lobbyChatUpdate;

    private readonly Action<ulong, bool> _onJoinedLobby;
    private readonly Action<ulong> _onCreatedLobby;
    private readonly Action<ulong> _onUserJoinedLobby;
    private readonly Action<ulong> _onUserLeftLobby;

    private bool _isHost;

    public SteamworksLobbyAPI(Action<ulong> onCreatedLobby, Action<ulong, bool> onJoinedLobby,
        Action<ulong> onUserJoinedLobby, Action<ulong> onUserLeftLobby)
    {
        if (!SteamManager.Initialized) throw new Exception("Steam is not open.");

        _onCreatedLobby = onCreatedLobby;
        _onJoinedLobby = onJoinedLobby;
        _onUserJoinedLobby = onUserJoinedLobby;
        _onUserLeftLobby = onUserLeftLobby;
        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        _lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        _lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
    }

    /// <summary>
    /// Detects change in the chatroom. Used to detect when player enter and exit the lobby.
    /// </summary>
    private void OnLobbyChatUpdate(LobbyChatUpdate_t callback)
    {
        switch ((EChatMemberStateChange)callback.m_rgfChatMemberStateChange) {
            case EChatMemberStateChange.k_EChatMemberStateChangeEntered:
                _onUserJoinedLobby(callback.m_ulSteamIDUserChanged);
                break;

            case EChatMemberStateChange.k_EChatMemberStateChangeLeft:
                _onUserLeftLobby(callback.m_ulSteamIDUserChanged);
                break;
        }
    }

    /// <summary>
    /// Called from client when using the join game feature in steam. 
    /// </summary>
    private void OnJoinRequested(GameLobbyJoinRequested_t callback)
    {
        JoinLobby(callback.m_steamIDLobby.m_SteamID);
    }

    /// <summary>
    /// Called by host when created a lobby.
    /// </summary>
    private async void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) {
            return;
        }

        ulong lobbyId = callback.m_ulSteamIDLobby;
        string networkAddress = SteamUser.GetSteamID().ToString();
        string name = $"{SteamFriends.GetPersonaName()}'s lobby";

        SteamMatchmaking.SetLobbyData(new CSteamID(lobbyId), SteamworksConsts.HostAddressKey, networkAddress);
        SteamMatchmaking.SetLobbyData(new CSteamID(lobbyId), SteamworksConsts.LobbyNameKey, name);

        // delay because SetLobbyData takes some time.
        // it's a quick fix so the other clients that are not in the lobby get the values set here
        await Task.Delay(500);

        _onCreatedLobby.Invoke(lobbyId);
    }

    /// <summary>
    /// Called when entering a lobby (for the host, after creation, and for the client when they join).
    /// </summary>
    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        if (callback.m_EChatRoomEnterResponse != (uint)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess) {
            return;
        }

        ulong lobbyId = callback.m_ulSteamIDLobby;

        _onJoinedLobby.Invoke(lobbyId, _isHost);
    }

    public void LeaveLobby(ulong id)
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(id));
    }

    public void JoinLobby(ulong lobbyId)
    {
        _isHost = false;

        SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
    }

    public void HostLobby(int maxConnections)
    {
        _isHost = true;

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, maxConnections);
    }
}