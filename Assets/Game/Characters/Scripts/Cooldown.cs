using UnityEngine;

public class Cooldown
{
    public Listenable<float> CurrentValue { get; } = new();
    public float MaxValue { get; }

    public bool IsReady => CurrentValue.Value <= 0;

    public Cooldown(float maxValue)
    {
        MaxValue = maxValue;
    }

    public void Start()
    {
        CurrentValue.Value = MaxValue;
    }

    public void Update()
    {
        if (CurrentValue.Value <= 0) return;

        CurrentValue.Value = Mathf.Clamp(CurrentValue.Value - Time.deltaTime, 0, MaxValue);
    }
}