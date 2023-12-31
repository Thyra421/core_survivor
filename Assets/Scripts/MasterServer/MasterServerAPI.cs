public class MasterServerAPI
{
    private readonly TcpTransport _tcpTransport;
    private readonly MessageRegistry _messageRegistry;

    public MasterServerAPI(string address, ushort port, MessageRegistry registry)
    {
        _tcpTransport = new TcpTransport(registry);
        _tcpTransport.Connect(address, port);
    }

    public void Send(MessageBase message)
    {
        _tcpTransport.Send(message);
    }
}