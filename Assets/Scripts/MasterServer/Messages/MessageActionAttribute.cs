using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class MessageActionAttribute : Attribute
{
    public string Action { get; }

    public MessageActionAttribute(string tag)
    {
        Action = tag;
    }
}