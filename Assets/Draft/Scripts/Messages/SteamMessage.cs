public class SteamMessage : MessageBase
{
    public SteamMessage()
    {
        action = GetType().ToString();
    }
}