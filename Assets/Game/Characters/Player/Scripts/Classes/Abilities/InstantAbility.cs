using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class InstantAbility : AbilityBase
{
    [SerializeField]
    protected float abilityDuration;

    [SerializeField]
    protected float delay;

    public bool IsCompleted { get; protected set; } = true;
    public override bool IsInProgress => !IsCompleted;

    public virtual void ClientUse()
    {
        IsCompleted = false;
        player.StartCoroutine(ResetIsCompletedCoroutine());
    }

    public virtual void ServerUse()
    {
        IsCompleted = false;
        player.StartCoroutine(ResetIsCompletedCoroutine());
        player.Class.Radioactivity.Decrease(cost);
    }

    private IEnumerator ResetIsCompletedCoroutine()
    {
        yield return new WaitForSeconds(abilityDuration);
        IsCompleted = true;
    }
}