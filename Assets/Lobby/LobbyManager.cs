public class LobbyManager : Singleton<LobbyManager>
{
    private SteamworksAPI _steamworksAPI;

    void OnCreatedAndJoinedLobby(SteamLobby lobby)
    {
        MasterServer.Current.Send(new CreateLobbyMessage {
            name = lobby.LobbyId.ToString(),
            ip = lobby.NetworkAddress,
            port = 123,
            action = "create"
        });
    }

    void OnJoinedLobby(SteamLobby lobby)
    {
        MasterServer.Current.Send(new CreateLobbyMessage {
            name = lobby.LobbyId.ToString(),
            ip = lobby.NetworkAddress,
            port = 123,
            action = "create"
        });
    }

    public void CreateAndJoinLobby()
    {
        _steamworksAPI.CreateAndJoinLobby(4, OnCreatedAndJoinedLobby);
    }

    public void JoinLobby(ulong lobbyId)
    {
        _steamworksAPI.JoinLobby(lobbyId, OnJoinedLobby);
    }

    protected override void Awake()
    {
        base.Awake();
        _steamworksAPI = new SteamworksAPI();
    }
}