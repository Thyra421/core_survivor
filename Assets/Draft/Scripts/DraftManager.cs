using Steamworks;

public class DraftManager : Singleton<DraftManager>
{
    private readonly MessageRegistry _messageRegistry = new();
    private SteamworksMessagingAPI _steamworksMessaging;

    public void StartGame()
    {
        MasterServer.Current.DeleteLobby();
        _steamworksMessaging.Send(new MessageStartGame());
    }

    public void LeaveDraft()
    {
        LobbyManager.Current.LeaveLobby();
        SceneLoader.Current.LoadMenuAsync();
    }

    public void PickDemolisher()
    {
        _steamworksMessaging.Send(new MessageSetClass(SteamUser.GetSteamID().m_SteamID, (int)Class.demolisher));
    }

    public void PickCannoneer()
    {
        _steamworksMessaging.Send(new MessageSetClass(SteamUser.GetSteamID().m_SteamID, (int)Class.cannoneer));
    }

    private void OnStartGame(MessageStartGame message)
    {
        SceneLoader.Current.LoadGameAsync();
    }

    private void OnSetClass(MessageSetClass message)
    {
        LobbyManager.Current.Players.Find(p => p.Id == message.Id).Class = (Class)message.Class;
        LobbyManager.Current.Players.NotifyListeners();
    }

    protected override void Awake()
    {
        base.Awake();
        _steamworksMessaging = new SteamworksMessagingAPI(LobbyManager.Current.LobbyId, _messageRegistry);

        _messageRegistry.AddListener<MessageStartGame>(OnStartGame);
        _messageRegistry.AddListener<MessageSetClass>(OnSetClass);
    }

    private void OnDestroy()
    {
        _messageRegistry.RemoveListener<MessageStartGame>(OnStartGame);
        _messageRegistry.RemoveListener<MessageSetClass>(OnSetClass);
    }
}