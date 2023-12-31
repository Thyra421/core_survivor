using Mirror;
using Mirror.FizzySteam;
using UnityEngine;

[RequireComponent(typeof(FizzySteamworks))]
public partial class Network : NetworkManager
{
    [SerializeField]
    private GameObject cannoneerPrefab;

    [SerializeField]
    private GameObject demolisherPrefab;

    [SerializeField]
    private GameObject[] enableOnStart;

    public override void Start()
    {
        networkAddress = LobbyManager.Current.NetworkAddress;

        if (LobbyManager.Current.IsHost) StartHost();

        else StartClient();

        foreach (GameObject o in enableOnStart) {
            o.SetActive(true);
        }

        MessageBroadcaster.Broadcast("GameStarted");
    }

    public void Stop()
    {
        if (LobbyManager.Current.IsHost) StopHost();

        else StopClient();
    }
}

public struct MessageSpawn : NetworkMessage
{
    public ulong Id;

    public MessageSpawn(ulong id)
    {
        Id = id;
    }
}