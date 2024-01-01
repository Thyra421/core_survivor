using System;
using System.Collections.Generic;

public class MessageRegistry
{
    private readonly Dictionary<Type, MessageListenerBase> _listeners = new();

    /// <summary>
    /// Adds a listener to the appropriate type.
    /// </summary>
    public void AddListener<T>(Action<T> onMessage) where T : MessageBase
    {
        Type type = typeof(T);

        if (!_listeners.ContainsKey(type))
            _listeners.Add(type, new MessageListener<T>());

        MessageListener<T> messageListener = _listeners[type] as MessageListener<T>;

        messageListener!.OnMessage += onMessage;
    }

    /// <summary>
    /// Removes a listener to the appropriate type.
    /// </summary>
    public void RemoveListener<T>(Action<T> onMessage) where T : MessageBase
    {
        Type type = typeof(T);

        if (!_listeners.ContainsKey(type)) return;

        MessageListener<T> messageListener = _listeners[type] as MessageListener<T>;

        messageListener!.OnMessage -= onMessage;

        if (messageListener!.IsEmpty) {
            _listeners.Remove(type);
        }
    }

    /// <summary>
    /// Invokes the event associated to the given type.
    /// Does nothing if no events are registered for this type.
    /// </summary>
    public void Invoke(object message, Type type)
    {
        if (!_listeners.ContainsKey(type)) return;

        _listeners[type].Invoke(message);
    }

    public void Clear()
    {
        _listeners.Clear();
    }
}