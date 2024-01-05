using System;
using UnityEngine;

[Serializable]
public abstract class AbilityBase
{
    [SerializeField]
    private Cooldown cooldown;

    [SerializeField]
    protected int cost;

    [SerializeField]
    protected int damages;

    [HideInInspector]
    public Player player;

    public Cooldown Cooldown => cooldown;
    public virtual bool CanUse => Cooldown.IsReady && player.Class.Radioactivity.Current.Value >= cost;
    public abstract bool IsInProgress { get; }

    public virtual void Update()
    {
        cooldown.Update();
    }
}