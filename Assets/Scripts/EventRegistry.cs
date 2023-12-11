using System;
using System.Collections.Generic;

public class EventRegistry<K, T>
{
    private Dictionary<K, EventListener<T>> _listeners = new Dictionary<K, EventListener<T>>();

    /// <summary>
    /// Adds a listener to the appropriate type.
    /// </summary>
    public void AddListener(K key, Action<T> listener) {
        if (!_listeners.ContainsKey(key))
            _listeners.Add(key, new EventListener<T>());

        _listeners[key].Listener += (T value) => listener(value);
    }

    /// <summary>
    /// Invokes the event associated to the given type.
    /// Does nothing if no events are registered for this type.
    /// </summary>
    public void Invoke(K key, T value) {
        if (_listeners.ContainsKey(key))
            _listeners[key].ListenerEvent?.Invoke(value);
    }

    public void Clear() {
        _listeners.Clear();
    }
}
