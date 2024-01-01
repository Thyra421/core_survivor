using System;
using System.Threading.Tasks;
using Steamworks;

public class SteamworksAPI
{
    // Assign callbacks to local variables to avoid garbage collection.
    protected readonly Callback<LobbyCreated_t> lobbyCreated;
    protected readonly Callback<GameLobbyJoinRequested_t> joinRequested;
    protected readonly Callback<LobbyEnter_t> lobbyEnter;

    private readonly Action<ulong, bool> _onJoinedLobby;
    private readonly Action<ulong> _onCreatedLobby;

    private bool _isHost = false;

    public SteamworksAPI(Action<ulong> onCreatedLobby, Action<ulong, bool> onJoinedLobby)
    {
        if (!SteamManager.Initialized) throw new Exception("Steam is not open.");

        string name = SteamFriends.GetPersonaName();
        ConsoleLogger.Steamworks($"Connected to Steam as {name}");

        _onCreatedLobby = onCreatedLobby;
        _onJoinedLobby = onJoinedLobby;
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
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
            ConsoleLogger.Steamworks("Creating lobby failed");
            return;
        }

        ConsoleLogger.Steamworks("Lobby created successfully");

        ulong lobbyId = callback.m_ulSteamIDLobby;
        string networkAddress = SteamUser.GetSteamID().ToString();
        string name = $"{SteamFriends.GetPersonaName()}'s lobby";

        SteamMatchmaking.SetLobbyData(new CSteamID(lobbyId), SteamworksConsts.HostAddressKey, networkAddress);
        SteamMatchmaking.SetLobbyData(new CSteamID(lobbyId), SteamworksConsts.LobbyNameKey, name);

        // delay because SetLobbyData takes some time.
        // it's a quick fix so the other clients that are not in the lobby get the values set here
        await Task.Delay(2000);

        _onCreatedLobby.Invoke(lobbyId);
    }

    /// <summary>
    /// Called when entering a lobby (for the host, after creation, and for the client when they join).
    /// </summary>
    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        if (callback.m_EChatRoomEnterResponse != (uint)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess) {
            ConsoleLogger.Steamworks("Entering lobby failed");
            return;
        }

        ConsoleLogger.Steamworks("Lobby entered successfully");

        ulong lobbyId = callback.m_ulSteamIDLobby;

        _onJoinedLobby.Invoke(lobbyId, _isHost);
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