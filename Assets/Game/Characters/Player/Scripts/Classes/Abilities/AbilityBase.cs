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

    [HideInInspector]
    public Player player;

    public Cooldown Cooldown => cooldown;
    public bool IsCompleted { get; protected set; } = true;
    public abstract bool IsChanneled { get; }

    public abstract void ClientUse(string args);
    public abstract void ServerUse(string args);
    public abstract void ClientEnd(string args);
    public abstract void ServerEnd(string args);
}