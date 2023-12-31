public class LobbyManager : Singleton<LobbyManager>
{
    private SteamworksAPI _steamworksAPI;

    public InstanceMode InstanceMode { get; private set; }
    public string NetworkAddress { get; private set; }

    private void OnCreatedLobby(SteamLobbyInformation lobbyInformation)
    {
        MasterServer.Current.Send(new CreateLobbyMessage {
            name = lobbyInformation.LobbyId.ToString(),
            ip = lobbyInformation.NetworkAddress,
            port = 123,
            action = "create"
        });
    }

    private void OnClientJoinedLobby(SteamLobbyInformation lobbyInformation)
    {
        NetworkAddress = lobbyInformation.NetworkAddress;
        InstanceMode = InstanceMode.Client;
        SceneLoader.Current.LoadGameAsync();
    }

    private void OnHostJoinedLobby(SteamLobbyInformation lobbyInformation)
    {
        NetworkAddress = lobbyInformation.NetworkAddress;
        InstanceMode = InstanceMode.Host;
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

    private void Start()
    {
        _steamworksAPI = new SteamworksAPI();
    }
}