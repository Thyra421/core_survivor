using Mirror;
using Mirror.FizzySteam;
using UnityEngine;

[RequireComponent(typeof(FizzySteamworks))]
public partial class Network : NetworkManager
{
    public override void Start()
    {
        // base.Start();
        //
        // GameInfo gameInfo = MasterServer.Current.GameInfo!;
        //
        // ((KcpTransport)transport).port = gameInfo.Port;
        // networkAddress = gameInfo.NetworkAddress;
        //
        // switch (gameInfo.InstanceMode) {
        //     case InstanceMode.Host:
        //         StartHost();
        //         break;
        //     case InstanceMode.Client:
        //         StartClient();
        //         break;
        // }
    }
}