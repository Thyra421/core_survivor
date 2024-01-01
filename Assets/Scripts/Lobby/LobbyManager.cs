using Steamworks;

public class LobbyManager : Singleton<LobbyManager>
{
    private SteamworksLobbyAPI _steamworksLobbyAPI;

    public ListenableList<LobbyPlayerInfo> Players = new();
    public ulong LobbyId { get; private set; }
    public bool IsHost { get; private set; }
    public string NetworkAddress { get; private set; }

    private void OnCreatedLobby(ulong id)
    {
        MasterServer.Current.CreateLobby(id);
    }

    private void OnJoinedLobby(ulong id, bool isHost)
    {
        SteamLobbyInformation steamLobbyInformation = SteamworksHelper.GetCurrentLobbyInformation(id);

        LobbyId = id;
        NetworkAddress = steamLobbyInformation.networkAddress;
        IsHost = isHost;

        SceneLoader.Current.LoadDraftAsync();
    }

    private void OnUserJoinedLobby(ulong id)
    {
        string username = SteamFriends.GetFriendPersonaName(new CSteamID(id));

        Players.Add(new LobbyPlayerInfo(id, username));
    }

    private void OnUserLeftLobby(ulong id)
    {
        int index = Players.FindIndex(p => p.Id == id);

        if (index == -1) return;

        Players.RemoveAt(index);
    }

    public void HostLobby()
    {
        _steamworksLobbyAPI.HostLobby(4);
    }

    public void JoinLobby(ulong lobbyId)
    {
        _steamworksLobbyAPI.JoinLobby(lobbyId);
    }

    public void LeaveLobby()
    {
        _steamworksLobbyAPI.LeaveLobby(LobbyId);
        if (IsHost)
            MasterServer.Current.DeleteLobby();
    }

    // ReSharper disable once UnusedMember.Local
    private void SteamStarted()
    {
        _steamworksLobbyAPI = new SteamworksLobbyAPI(OnCreatedLobby, OnJoinedLobby, OnUserJoinedLobby, OnUserLeftLobby);
    }
}