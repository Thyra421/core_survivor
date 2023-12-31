using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

internal class TcpTransport
{
    private TcpClient _tcpClient;
    private readonly MessageRegistry _messageRegistry;

    public TcpTransport(MessageRegistry messageRegistry)
    {
        _messageRegistry = messageRegistry;
    }

    private async void Listen()
    {
        if (_tcpClient is not { Connected: true }) {
            OnError();
        }

        OnListen();
        NetworkStream stream = _tcpClient!.GetStream();

        byte[] bytes = new byte[Config.Current.TCPChunkSize];

        try {
            int i;
            while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0) {
                string message = Encoding.ASCII.GetString(bytes, 0, i);
                OnMessage(message);
            }

            OnDisconnected();
        }
        catch (SocketException e) {
            Debug.LogException(e);
            OnDisconnected();
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public async void Send<T>(T message)
    {
        if (_tcpClient is not { Connected: true }) {
            OnError();
            return;
        }

        string serializedMessage = JsonUtility.ToJson(message);
        byte[] bytes = Encoding.ASCII.GetBytes(serializedMessage);

        await _tcpClient!.GetStream().WriteAsync(bytes, 0, bytes.Length);
    }

    private void OnMessage(string message)
    {
        ConsoleLogger.Master($"Received {message}");

        ServerMessageBase messageBase = JsonUtility.FromJson<ServerMessageBase>(message);
        Type messageType = MessageHelper.GetType(messageBase.action);

        object obj = JsonUtility.FromJson(message, messageType);

        _messageRegistry.Invoke(obj, messageType);
    }

    public bool Connect(string address, ushort port)
    {
        try {
            _tcpClient = new TcpClient(address, port);
            OnConnected();
            Listen();
            return true;
        }
        catch {
            OnConnectionFailed();
            return false;
        }
    }

    #region Callbacks

    private void OnError()
    {
        ConsoleLogger.Master($"Not connected");
    }

    private void OnListen()
    {
        ConsoleLogger.Master($"Listening");
    }

    private void OnDisconnected()
    {
        ConsoleLogger.Master($"Client disconnected");
    }

    private void OnConnectionFailed()
    {
        ConsoleLogger.Master($"Connection failed");
    }

    private void OnConnected()
    {
        ConsoleLogger.Master($"Connected");
    }

    #endregion
}