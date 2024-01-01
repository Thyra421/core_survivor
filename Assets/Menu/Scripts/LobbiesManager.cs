public class LobbiesManager : Singleton<LobbiesManager>
{
    public ListenableList<LobbyInformation> Lobbies { get; } = new();

    private void OnDeletedLobbyMessage(DeletedLobbyMessage message)
    {
        int index = Lobbies.FindIndex((r) => r.id == ulong.Parse(message.id));

        if (index == -1) return;

        Lobbies.RemoveAt(index);
    }

    private void OnCreatedLobbyMessage(CreatedLobbyMessage message)
    {
        AddLobby(ulong.Parse(message.id));
    }

    private void OnLobbiesMessage(LobbiesMessage message)
    {
        foreach (string id in message.id) {
            AddLobby(ulong.Parse(id));
        }
    }

    private async void AddLobby(ulong id)
    {
        try {
            SteamLobbyInformation steamLobbyInformation = await
                SteamworksHelper.GetOtherLobbyInformation(id);

            LobbyInformation lobbyInformation = new() {
                id = steamLobbyInformation.id,
                name = steamLobbyInformation.name
            };

            Lobbies.Add(lobbyInformation);
        }
        catch {
            // ignored
        }
    }

    protected override void Awake()
    {
        base.Awake();
        MasterServer.Current.AddListener<CreatedLobbyMessage>(OnCreatedLobbyMessage);
        MasterServer.Current.AddListener<DeletedLobbyMessage>(OnDeletedLobbyMessage);
        MasterServer.Current.AddListener<LobbiesMessage>(OnLobbiesMessage);
    }

    private void OnDestroy()
    {
        MasterServer.Current.RemoveListener<CreatedLobbyMessage>(OnCreatedLobbyMessage);
        MasterServer.Current.RemoveListener<DeletedLobbyMessage>(OnDeletedLobbyMessage);
        MasterServer.Current.RemoveListener<LobbiesMessage>(OnLobbiesMessage);
    }
}