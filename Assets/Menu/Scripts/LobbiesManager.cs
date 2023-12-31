public class LobbiesManager : Singleton<LobbiesManager>
{
    public ListenableList<LobbyInformations> Lobbies { get; } = new();

    private void OnDeleteLobbyMessage(DeleteLobbyMessage message)
    {
        int index = Lobbies.FindIndex((r) => r.name == message.name);

        if (index == -1) return;
        
        Lobbies.RemoveAt(index);
    }

    private void OnCreateLobbyMessage(CreateLobbyMessage message)
    {
        LobbyInformations lobbyInformations = new() {
            name = message.name,
            networkAddress = message.ip,
            port = message.port
        };
        Lobbies.Add(lobbyInformations);
    }

    protected void Start()
    {
        MasterServer.Current.AddListener<CreateLobbyMessage>(OnCreateLobbyMessage);
        MasterServer.Current.AddListener<DeleteLobbyMessage>(OnDeleteLobbyMessage);
    }
}