using System;
using System.Collections.Generic;

public class ListenableList<T> : List<T>
{
    public event Action<T> OnAddedElement;
    public event Action<T> OnRemovedElement;

    /// <summary>
    /// Add new value and notify listeners.
    /// </summary>
    public new void Add(T value)
    {
        base.Add(value);

        OnAddedElement?.Invoke(value);
    }

    /// <summary>
    /// Remove value and notify listeners.
    /// </summary>
    public new bool Remove(T value)
    {
        bool result = base.Remove(value);

        OnRemovedElement?.Invoke(value);

        return result;
    }
}