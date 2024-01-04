using Mirror;
using UnityEngine;

public partial class Network
{
    private void OnMessageSpawn(NetworkConnectionToClient conn, MessageSpawn message)
    {
        int index = LobbyManager.Current.Players.FindIndex(p => p.Id == message.Id);
        Class @class = LobbyManager.Current.Players[index].Class;

        GameObject newGameObject = Instantiate(@class == Class.cannoneer
            ? cannoneerPrefab
            : demolisherPrefab, GetStartPosition().position, Quaternion.identity);

        newGameObject.GetComponent<Player>().index = index;

        NetworkServer.AddPlayerForConnection(conn, newGameObject);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        ConsoleLogger.Server("Started");
        NetworkServer.RegisterHandler<MessageSpawn>(OnMessageSpawn);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        ConsoleLogger.Server("Stopped");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        ConsoleLogger.Server("Client connects");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        ConsoleLogger.Server("Client disconnects");
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        ConsoleLogger.Server("Client joined scene");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        ConsoleLogger.Server("Client requests to add player");
    }

    public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
    {
        base.OnServerError(conn, error, reason);
        ConsoleLogger.Server($"Error : {error}");
    }
}