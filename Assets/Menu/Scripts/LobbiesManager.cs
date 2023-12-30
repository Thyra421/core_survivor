public class LobbiesManager : Singleton<LobbiesManager>
{
    public ListenableList<Lobby> Lobbies { get; } = new();

    private void OnDeleteLobbyMessage(DeleteLobbyMessage message)
    {
        int index = Lobbies.FindIndex((r) => r.name == message.name);
        Lobbies.RemoveAt(index);
    }

    private void OnCreateLobbyMessage(CreateLobbyMessage message)
    {
        Lobby lobby = new() {
            name = message.name,
            networkAddress = message.ip,
            port = message.port
        };
        Lobbies.Add(lobby);
    }

    protected void Start()
    {
        MasterServer.Current.AddListener<CreateLobbyMessage>(OnCreateLobbyMessage);
        MasterServer.Current.AddListener<DeleteLobbyMessage>(OnDeleteLobbyMessage);
    }
}