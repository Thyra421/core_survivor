using System;
using System.Collections.Generic;

public class ListenableList<T> : List<T>
{
    public event Action<List<T>> OnValueChanged;
        
    /// <summary>
    /// Add new value and notify listeners.
    /// </summary>
    public new void Add(T value)
    {
        base.Add(value);
        OnValueChanged?.Invoke(this);
    }
    
    /// <summary>
    /// Remove value and notify listeners.
    /// </summary>
    public new void Remove(T value)
    {
        base.Remove(value);
        OnValueChanged?.Invoke(this);
    }
}