using System;
using UnityEngine;

[Serializable]
public abstract class AbilityBase
{
    [SerializeField]
    protected float abilityDuration;

    [SerializeField]
    protected float delay;

    [SerializeField]
    private Cooldown cooldown;

    public Cooldown Cooldown => cooldown;
    public bool IsCompleted { get; protected set; } = true;

    public abstract void ClientUse(string args);
    public abstract void ServerUse(string args);
}