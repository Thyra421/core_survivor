public class MessageSetClass : SteamMessage
{
    public ulong Id;
    
    public int Class;

    public MessageSetClass(ulong id, int @class)
    {
        Id = id;
        Class = @class;
    }
}