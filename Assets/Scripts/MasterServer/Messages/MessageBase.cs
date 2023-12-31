using System;

public abstract class MessageBase
{
    public string action;
}

public class ClientMessageBase : MessageBase
{
    protected ClientMessageBase()
    {
        MessageActionAttribute attribute =
            (MessageActionAttribute)Attribute.GetCustomAttribute(GetType(),
                typeof(MessageActionAttribute));

        if (attribute == null) throw new Exception("Missing action attribute in client message.");

        action = attribute.Action;
    }
}

public class ServerMessageBase : MessageBase { }