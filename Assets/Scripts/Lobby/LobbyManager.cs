public class LobbyManager : Singleton<LobbyManager>
{
    private SteamworksLobbyAPI _steamworksLobbyAPI;

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
        MasterServer.Current.DeleteLobby();
    }

    // ReSharper disable once UnusedMember.Local
    private void SteamStarted()
    {
        _steamworksLobbyAPI = new SteamworksLobbyAPI(OnCreatedLobby, OnJoinedLobby);
    }
}