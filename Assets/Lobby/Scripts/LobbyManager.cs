public class LobbyManager : Singleton<LobbyManager>
{
    private SteamworksAPI _steamworksAPI;

    public bool IsHost { get; private set; }
    public string NetworkAddress { get; private set; }

    private void OnCreatedLobby(ulong id)
    {
        MasterServer.Current.CreateLobby(id);
    }

    private void OnClientJoinedLobby(ulong id)
    {
        SteamLobbyInformation steamLobbyInformation = SteamworksHelper.GetLobbyInformation(id);

        NetworkAddress = steamLobbyInformation.networkAddress;
        IsHost = false;

        SceneLoader.Current.LoadGameAsync();
    }

    private void OnHostJoinedLobby(ulong id)
    {
        SteamLobbyInformation steamLobbyInformation = SteamworksHelper.GetLobbyInformation(id);

        NetworkAddress = steamLobbyInformation.networkAddress;
        IsHost = true;

        SceneLoader.Current.LoadGameAsync();
    }

    public void CreateAndJoinLobby()
    {
        _steamworksAPI.CreateAndJoinLobby(4, OnCreatedLobby, OnHostJoinedLobby);
    }

    public void JoinLobby(ulong lobbyId)
    {
        _steamworksAPI.JoinLobby(lobbyId, OnClientJoinedLobby);
    }

    protected override void Awake()
    {
        base.Awake();
        _steamworksAPI = new SteamworksAPI();
    }
}