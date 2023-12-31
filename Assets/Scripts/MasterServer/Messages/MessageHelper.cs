using System;
using System.Collections.Generic;
using System.Linq;

internal static class MessageHelper
{
    private static readonly Dictionary<string, Type> TagToTypeMap = new();

    static MessageHelper()
    {
        IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => Attribute.IsDefined(p, typeof(MessageActionAttribute)));

        foreach (Type type in types) {
            MessageActionAttribute attribute =
                (MessageActionAttribute)Attribute.GetCustomAttribute(type, typeof(MessageActionAttribute));
            if (attribute != null) {
                TagToTypeMap[attribute.Action] = type;
            }
        }
    }

    public static Type GetType(string tag)
    {
        if (TagToTypeMap.TryGetValue(tag, out Type type)) {
            return type;
        }

        throw new ArgumentException($"No class found with the tag: {tag}");
    }
}