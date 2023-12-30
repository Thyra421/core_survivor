using System;
using System.Collections.Generic;

public class MessageRegistry
{
    private Dictionary<Type, MessageListener> _listeners = new();

    /// <summary>
    /// Adds a listener to the appropriate type.
    /// </summary>
    public void AddListener<T>(Action<T> onMessage)
    {
        Type type = typeof(T);

        if (!_listeners.ContainsKey(type))
            _listeners.Add(type, new MessageListener());

        _listeners[type].OnMessage += (obj) => onMessage((T)obj);
    }

    /// <summary>
    /// Invokes the event associated to the given type.
    /// Does nothing if no events are registered for this type.
    /// </summary>
    public void Invoke(object message, Type type)
    {
        if (_listeners.ContainsKey(type))
            _listeners[type].OnMessageEvent?.Invoke(message);
    }

    public void Clear()
    {
        _listeners.Clear();
    }
}