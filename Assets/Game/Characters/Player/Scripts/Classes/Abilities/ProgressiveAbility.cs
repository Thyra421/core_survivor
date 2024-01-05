using System;
using UnityEngine;

[Serializable]
public abstract class ProgressiveAbility : AbilityBase
{
    [SerializeField]
    private Cooldown interval;

    public bool IsUsing { get; protected set; }
    public override bool CanUse => base.CanUse && interval.IsReady;
    public override bool IsInProgress => IsUsing;

    public virtual void ClientStartUsing()
    {
        IsUsing = true;
        interval.Start();
    }

    public virtual void ServerStartUsing()
    {
        IsUsing = true;
        interval.Start();
    }

    protected virtual void ClientUsing()
    {
        interval.Start();
    }

    protected virtual void ServerUsing()
    {
        interval.Start();
        player.Class.Radioactivity.Decrease(cost);
    }

    public virtual void ClientStopUsing()
    {
        IsUsing = false;
        Cooldown.Start();
    }

    public virtual void ServerStopUsing()
    {
        IsUsing = false;
        Cooldown.Start();
    }

    public override void Update()
    {
        base.Update();
        interval.Update();

        if (!(IsUsing && CanUse)) return;

        if (!player.isServer) return;
        
        ServerUsing();
        ClientUsing();
    }
}