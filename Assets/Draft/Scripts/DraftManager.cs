using Steamworks;

public class DraftManager : Singleton<DraftManager>
{
    private readonly MessageRegistry _messageRegistry = new();
    private SteamworksMessagingAPI _steamworksMessaging;

    public void StartGame()
    {
        _steamworksMessaging.Send(new MessageStartGame());
    }

    public void LeaveDraft()
    {
        LobbyManager.Current.LeaveLobby();
        SceneLoader.Current.LoadMenuAsync();
    }

    protected override void Awake()
    {
        base.Awake();
        _steamworksMessaging = new SteamworksMessagingAPI(LobbyManager.Current.LobbyId, _messageRegistry);
    }
}