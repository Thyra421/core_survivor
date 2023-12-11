using Mirror;

public partial class Network
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        Log.Server("Started");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Log.Server("Stopped");
    }
    
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Log.Server("Client connects");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Log.Server("Client disconnects");
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        Log.Server("Client joined scene");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        Log.Server("Client requests to add player");
    }

    public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
    {
        base.OnServerError(conn, error, reason);
        Log.Server($"Error : {error}");
    }
}