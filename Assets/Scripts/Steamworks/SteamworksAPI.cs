using System;
using Steamworks;

public class SteamworksAPI
{
    // Assign callbacks to local variables to avoid garbage collection.
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequested;
    protected Callback<LobbyEnter_t> lobbyEnter;

    private Action<ulong> _onJoinedLobby;
    private Action<ulong> _onCreatedLobby;

    public SteamworksAPI()
    {
        if (!SteamManager.Initialized) throw new Exception("Steam is not open.");

        string name = SteamFriends.GetPersonaName();
        ConsoleLogger.Steamworks($"Connected to Steam as {name}");

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
    }

    /// <summary>
    /// Called from client when using the join game feature in steam. 
    /// </summary>
    private void OnJoinRequested(GameLobbyJoinRequested_t callback)
    {
        // JoinLobby(callback.m_steamIDLobby.m_SteamID);
    }

    /// <summary>
    /// Called by host when created a lobby.
    /// </summary>
    private void OnLobbyCreated(LobbyCreated_t callback)
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

        _onJoinedLobby.Invoke(lobbyId);
    }

    public void JoinLobby(ulong lobbyId, Action<ulong> onJoinedLobby)
    {
        _onJoinedLobby = onJoinedLobby;

        SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
    }

    public void CreateAndJoinLobby(int maxConnections, Action<ulong> onCreatedLobby,
        Action<ulong> onJoinedLobby)
    {
        _onCreatedLobby = onCreatedLobby;
        _onJoinedLobby = onJoinedLobby;

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, maxConnections);
    }
}