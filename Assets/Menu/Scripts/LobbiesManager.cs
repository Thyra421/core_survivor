public class LobbiesManager : Singleton<LobbiesManager>
{
    public ListenableList<LobbyInformation> Lobbies { get; } = new();

    private void OnDeletedLobbyMessage(DeletedLobbyMessage message)
    {
        int index = Lobbies.FindIndex((r) => r.id == ulong.Parse(message.id));

        if (index == -1) return;

        Lobbies.RemoveAt(index);
    }

    private async void OnCreatedLobbyMessage(CreatedLobbyMessage message)
    {
        SteamLobbyInformation steamLobbyInformation = await
            SteamworksHelper.GetOtherLobbyInformation(ulong.Parse(message.id));

        LobbyInformation lobbyInformation = new() {
            id = steamLobbyInformation.id,
            name = steamLobbyInformation.name
        };

        Lobbies.Add(lobbyInformation);
    }

    protected void Start()
    {
        MasterServer.Current.AddListener<CreatedLobbyMessage>(OnCreatedLobbyMessage);
        MasterServer.Current.AddListener<DeletedLobbyMessage>(OnDeletedLobbyMessage);
    }

    private void OnDestroy()
    {
        MasterServer.Current.RemoveListener<CreatedLobbyMessage>(OnCreatedLobbyMessage);
        MasterServer.Current.RemoveListener<DeletedLobbyMessage>(OnDeletedLobbyMessage);
    }
}