using Mirror;
using Mirror.FizzySteam;
using UnityEngine;

[RequireComponent(typeof(FizzySteamworks))]
public partial class Network : NetworkManager
{
    public override void Start()
    {
        networkAddress = LobbyManager.Current.NetworkAddress;

        if (LobbyManager.Current.IsHost) StartHost();

        else StartClient();

        MessageBroadcaster.Broadcast("GameStarted");
    }

    public void Stop()
    {
        if (LobbyManager.Current.IsHost) StopHost();

        else StopClient();
    }
}