using Mirror;

public partial class Network
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        Log.Client("Started");
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        Log.Client("Stopped");
    }
    
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Log.Client("Connected to server");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Log.Client("Disconnected to server");
    }

    public override void OnClientError(TransportError error, string reason)
    {
        base.OnClientError(error, reason);
        Log.Client($"Error : {error}");
    }

    public override void OnClientNotReady()
    {
        base.OnClientNotReady();
        Log.Client("Not ready");
    }
}
