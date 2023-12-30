public class GameInfo
{
    public GameInfo(InstanceMode instanceMode, ushort port, string networkAddress)
    {
        Port = port;
        InstanceMode = instanceMode;
        NetworkAddress = networkAddress;
    }

    public InstanceMode InstanceMode { get; }

    public ushort Port { get; }

    public string NetworkAddress { get; }
}