public class MessageListener
{
    public delegate void OnMessageHandler(object message);

    public event OnMessageHandler OnMessage;

    public OnMessageHandler OnMessageEvent => OnMessage;
}