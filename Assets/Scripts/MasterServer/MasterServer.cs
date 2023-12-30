using System;
using JetBrains.Annotations;
using UnityEngine;

public class MasterServer : Singleton<MasterServer>
{
    [SerializeField] private string serverAddress;
    [SerializeField] private ushort serverPort;

    private readonly MessageRegistry _messageRegistry = new();
    private TcpTransport _tcpTransport;

    [CanBeNull] public GameInfo GameInfo { get; set; }

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

public enum InstanceMode
{
    Host,
    Client
}