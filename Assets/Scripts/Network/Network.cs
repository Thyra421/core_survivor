using System;
using Mirror;
using Mirror.FizzySteam;
using UnityEngine;

[RequireComponent(typeof(FizzySteamworks))]
public partial class Network : NetworkManager
{
    public override void Start()
    {
        if (LobbyManager.Current == null) throw new Exception("Not in a lobby");

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