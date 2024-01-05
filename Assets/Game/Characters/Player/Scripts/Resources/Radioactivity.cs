using System;

public class Radioactivity
{
    public const int MaxValue = 100;
    public Listenable<int> Current { get; } = new();
    public bool IsFull => Current.Value == MaxValue;
    public bool IsEmpty => Current.Value == 0;

    public void Increase(int amount)
    {
        Current.Value = Math.Clamp(Current.Value + amount, 0, MaxValue);
    }

    public void Decrease(int amount)
    {
        Current.Value = Math.Clamp(Current.Value - amount, 0, MaxValue);
    }

    public void Empty()
    {
        Current.Value = 0;
    }

    public void Fill()
    {
        Current.Value = MaxValue;
    }
}