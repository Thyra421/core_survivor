[MessageAction("create")]
public class CreateLobbyMessage : MessageBase
{
    public string name;
    public string ip;
    public ushort port;
}