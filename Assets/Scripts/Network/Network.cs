using Mirror;
using Mirror.FizzySteam;
using UnityEngine;

[RequireComponent(typeof(FizzySteamworks))]
public partial class Network : NetworkManager
{
    public override void Start()
    {
        if (LobbyManager.Current == null) {
            Debug.LogError("Not in a lobby");
            return;
        }

        networkAddress = LobbyManager.Current.NetworkAddress;

        switch (LobbyManager.Current.InstanceMode) {
            case InstanceMode.Host:
                StartHost();
                break;
            case InstanceMode.Client:
                StartClient();
                break;
        }

        foreach (GameObject go in (GameObject[])FindObjectsOfType(typeof(GameObject))) {
            go.gameObject.BroadcastMessage("StartGame", null, SendMessageOptions.DontRequireReceiver);
        }
    }
}