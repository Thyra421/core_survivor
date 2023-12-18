using kcp2k;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(KcpTransport))]
public partial class Network : NetworkManager
{
    public void Initialize(bool masterPort = false)
    {
        switch (InstanceModeManager.Mode)
        {
            case InstanceMode.Client:
                ((KcpTransport)transport).port = masterPort ? (ushort)7777 : (ushort)InstanceModeManager.ServerPort!;
                StartClient();
                break;
            case InstanceMode.MasterServer:
                StartServer();
                break;
            case InstanceMode.DedicatedServer:
                ((KcpTransport)transport).port = (ushort)InstanceModeManager.ServerPort!;
                StartServer();
                break;
        }
    }
}