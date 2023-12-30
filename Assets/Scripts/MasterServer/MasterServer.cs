using System;
using UnityEngine;

public class MasterServer : Singleton<MasterServer>
{
    [SerializeField] private string serverAddress;
    [SerializeField] private ushort serverPort;

    private readonly MessageRegistry _messageRegistry = new();
    private TcpTransport _tcpTransport;

    public void Send(MessageBase message)
    {
        _tcpTransport.Send(message);
    }

    public void AddListener<T>(Action<T> listener) where T : MessageBase
    {
        _messageRegistry.AddListener(listener);
    }

    protected override void Awake()
    {
        base.Awake();
        _tcpTransport = new TcpTransport(_messageRegistry);
        _tcpTransport.Connect(serverAddress, serverPort);
    }
}