public class LobbiesManager : Singleton<LobbiesManager>
{
    public ListenableList<LobbyInformation> Lobbies { get; } = new();

    private void OnDeletedLobbyMessage(DeletedLobbyMessage message)
    {
        int index = Lobbies.FindIndex((r) => r.name == message.id);

        if (index == -1) return;

        Lobbies.RemoveAt(index);
    }

    private void OnCreatedLobbyMessage(CreatedLobbyMessage message)
    {
        SteamLobbyInformation steamLobbyInformation =
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
}