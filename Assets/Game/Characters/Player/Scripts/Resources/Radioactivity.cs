using System;

public class Radioactivity
{
    private const int MAX_VALUE = 100;

    public Listenable<int> Value { get; } = new();
    public bool IsFull => Value.Value == MAX_VALUE;
    public bool IsEmpty => Value.Value == 0;

    public void Increase(int amount)
    {
        Value.Value = Math.Clamp(Value.Value + amount, 0, MAX_VALUE);
    }

    public void Decrease(int amount)
    {
        Value.Value = Math.Clamp(Value.Value - amount, 0, MAX_VALUE);
    }

    public void Empty()
    {
        Value.Value = 0;
    }

    public void Fill()
    {
        Value.Value = MAX_VALUE;
    }
}