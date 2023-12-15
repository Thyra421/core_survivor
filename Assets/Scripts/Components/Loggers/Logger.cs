using UnityEngine;

public static class ConsoleLogger
{
    public static void Server(object message)
    {
        Debug.Log($"<color=#00a6ff>[SERVER] {message}</color>");
    }
    
    public static void Client(object message)
    {
        Debug.Log($"<color=#ffae00>[CLIENT] {message}</color>");
    }
}