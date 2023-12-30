[MessageAction("create")]
public class CreateRoomMessage : MessageBase
{
    public string name;
    public string ip;
    public ushort port;
}