using UnityEngine;

public static class MessageBroadcaster
{
    public static void Broadcast(string methodName)
    {
        foreach (GameObject go in (GameObject[])Object.FindObjectsOfType(typeof(GameObject))) {
            go.BroadcastMessage(methodName, null, SendMessageOptions.DontRequireReceiver);
        }
    }
}