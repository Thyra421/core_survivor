public static class InstanceModeManager
{
    /// <summary>
    /// The mode of the current instance of the game.
    /// </summary>
    public static InstanceMode Mode { get; set; }
    
    /// <summary>
    /// The port of the server to connect to if it's a client or the port to use if it's a dedicated server. 
    /// </summary>
    public static int? ServerPort { get; set; }
}

public enum InstanceMode
{
    MasterServer,
    DedicatedServer,
    Client
}