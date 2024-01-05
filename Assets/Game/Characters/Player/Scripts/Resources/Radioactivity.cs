using System;

public class Radioactivity
{
    public const int MaxValue = 100;
    public Listenable<int> Current { get; } = new();
    public bool IsFull => Current.Value == MaxValue;
    public bool IsEmpty => Current.Value == 0;
    private Action<int> _onChanged;

    public Radioactivity(Action<int> onChanged)
    {
        _onChanged = onChanged;
    }

    public void Increase(int amount)
    {
        Current.Value = Math.Clamp(Current.Value + amount, 0, MaxValue);
        _onChanged(Current.Value);
    }

    public void Decrease(int amount)
    {
        Current.Value = Math.Clamp(Current.Value - amount, 0, MaxValue);
        _onChanged(Current.Value);
    }

    public void Empty()
    {
        Current.Value = 0;
        _onChanged(Current.Value);
    }

    public void Fill()
    {
        Current.Value = MaxValue;
        _onChanged(Current.Value);
    }
}