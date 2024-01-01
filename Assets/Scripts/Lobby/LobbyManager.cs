public class LobbyManager : Singleton<LobbyManager>
{
    private SteamworksLobbyAPI _steamworksLobbyAPI;

    public bool IsHost { get; private set; }
    public string NetworkAddress { get; private set; }

    private void OnCreatedLobby(ulong id)
    {
        MasterServer.Current.CreateLobby(id);
    }

    private void OnJoinedLobby(ulong id, bool isHost)
    {
        SteamLobbyInformation steamLobbyInformation = SteamworksHelper.GetCurrentLobbyInformation(id);

        NetworkAddress = steamLobbyInformation.networkAddress;
        IsHost = isHost;

        SceneLoader.Current.LoadGameAsync();
    }

    public void CreateAndJoinLobby()
    {
        _steamworksLobbyAPI.HostLobby(4);
    }

    public void JoinLobby(ulong lobbyId)
    {
        _steamworksLobbyAPI.JoinLobby(lobbyId);
    }

    protected override void Awake()
    {
        base.Awake();
        _steamworksLobbyAPI = new SteamworksLobbyAPI(OnCreatedLobby, OnJoinedLobby);
    }
}