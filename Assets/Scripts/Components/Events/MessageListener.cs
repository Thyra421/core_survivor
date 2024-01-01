using System;

public abstract class MessageListenerBase
{
    public abstract void Invoke(object messageBase);
}

public class MessageListener<T> : MessageListenerBase where T : MessageBase
{
    public event Action<T> OnMessage;

    public bool IsEmpty => OnMessage == null || OnMessage.GetInvocationList().Length == 0;

    public override void Invoke(object messageBase)
    {
        T message = messageBase as T;

        OnMessage?.Invoke(message);
    }
}