using System;
using System.Collections.Generic;
using UnityEngine;

public class ListenableList<T> : List<T>
{
    public event Action<ListenableList<T>> OnChanged;

    /// <summary>
    /// Add new value and notify listeners.
    /// </summary>
    public new void Add(T value)
    {
        base.Add(value);

        OnChanged?.Invoke(this);
    }

    /// <summary>
    /// Remove value and notify listeners.
    /// </summary>
    public new bool Remove(T value)
    {
        bool result = base.Remove(value);

        OnChanged?.Invoke(this);

        return result;
    }

    /// <summary>
    /// Remove value and notify listeners.
    /// </summary>
    public new void RemoveAt(int index)
    {
        base.RemoveAt(index);

        OnChanged?.Invoke(this);
    }
}