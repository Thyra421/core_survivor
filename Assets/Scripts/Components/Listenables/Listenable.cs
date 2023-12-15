using System;

/// <summary>
/// Listenable object to which you can add listeners and get informed when the value changes.
/// To listen to lists use ListenableList instead.
/// </summary>
/// <typeparam name="T">Type</typeparam>
public class Listenable<T>
{
    private T _value;

    public event Action<T> OnValueChanged;

    public Listenable(T initialValue)
    {
        _value = initialValue;
    }

    public Listenable()
    {
    }

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChanged?.Invoke(value);
        }
    }
}