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

    public void CreateLobby(ulong id)
    {
        _api.Send(new CreateLobbyMessage { id = id.ToString() });
    }

    protected override void Awake()
    {
        base.Awake();
        _api = new MasterServerAPI(serverAddress, serverPort, _messageRegistry);
    }
}