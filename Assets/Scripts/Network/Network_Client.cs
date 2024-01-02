using Mirror;
using UnityEngine;

public partial class Network
{
    [SerializeField] private GameObject disconnectedFromServerPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();
        ConsoleLogger.Client("Started");
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        ConsoleLogger.Client("Stopped");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ConsoleLogger.Client("Connected to server");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        ConsoleLogger.Client("Disconnected from server");
        
        LobbyManager.Current.LeaveLobby();
        Instantiate(disconnectedFromServerPrefab);
    }

    public override void OnClientError(TransportError error, string reason)
    {
        base.OnClientError(error, reason);
        ConsoleLogger.Client($"Error : {error}");
    }

    public override void OnClientNotReady()
    {
        base.OnClientNotReady();
        ConsoleLogger.Client("Not ready");
    }
}