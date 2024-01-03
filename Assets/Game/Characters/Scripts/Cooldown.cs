using System;
using UnityEngine;

[Serializable]
public class Cooldown
{
    [SerializeField]
    private float duration;

    public Listenable<float> CurrentValue { get; } = new(0);

    public bool IsReady => CurrentValue.Value <= 0;

    public float ProgressRatio => CurrentValue.Value / duration;

    public void Start()
    {
        CurrentValue.Value = duration;
    }

    public void Update()
    {
        if (CurrentValue.Value <= 0) return;

        CurrentValue.Value = Mathf.Clamp(CurrentValue.Value - Time.deltaTime, 0, duration);
    }
}