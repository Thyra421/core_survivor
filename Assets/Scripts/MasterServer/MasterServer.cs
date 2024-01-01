using System;
using UnityEngine;

public class MasterServer : Singleton<MasterServer>
{
    [SerializeField] private string serverAddress;
    [SerializeField] private ushort serverPort;

    private readonly MessageRegistry _messageRegistry = new();
    private MasterServerAPI _api;

    public void AddListener<T>(Action<T> listener) where T : MessageBase
    {
        _messageRegistry.AddListener(listener);
    }

    public void RemoveListener<T>(Action<T> listener) where T : MessageBase
    {
        _messageRegistry.RemoveListener(listener);
    }

    public void CreateLobby(ulong id)
    {
        if (_api == null) throw new Exception("Steam API is not initialized");
        
        _api.Send(new CreateLobbyMessage { id = id.ToString() });
    }

    // ReSharper disable once UnusedMember.Local
    private void SteamStarted()
    {
        _api = new MasterServerAPI(serverAddress, serverPort, _messageRegistry);
    }
}