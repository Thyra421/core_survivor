using System;
using System.Collections.Generic;
using System.Linq;

static class MessageHelper
{
    private static readonly Dictionary<string, Type> tagToTypeMap = new();

    static MessageHelper()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => Attribute.IsDefined(p, typeof(MessageActionAttribute)));

        foreach (var type in types) {
            var attribute = (MessageActionAttribute)Attribute.GetCustomAttribute(type, typeof(MessageActionAttribute));
            if (attribute != null) {
                tagToTypeMap[attribute.Action] = type;
            }
        }
    }

    public static Type GetType(string tag)
    {
        if (tagToTypeMap.TryGetValue(tag, out Type type)) {
            return type;
        }

        throw new ArgumentException($"No class found with the tag: {tag}");
    }
}