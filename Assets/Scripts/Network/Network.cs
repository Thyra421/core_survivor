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

        if (LobbyManager.Current.IsHost) StartHost();

        else StartClient();

        foreach (GameObject go in (GameObject[])FindObjectsOfType(typeof(GameObject))) {
            go.gameObject.BroadcastMessage("StartGame", null, SendMessageOptions.DontRequireReceiver);
        }
    }
}