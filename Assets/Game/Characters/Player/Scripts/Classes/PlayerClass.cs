using System;
using System.Linq;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Player))]
public abstract class PlayerClass : NetworkBehaviour
{
    [CanBeNull]
    private AbilityBase _currentAbility;

    public AbilityBase[] Abilities { get; protected set; }
    public Radioactivity Radioactivity { get; private set; }
    public Vector3 Target { get; private set; }
    public bool IsBusy => Abilities.Any(a => a.IsInProgress);

    protected bool CanUseAbility(int index)
    {
        return !IsBusy && Abilities[index].CanUse;
    }

    #region Radioactivity

    [Server]
    private void OnRadioactivityChanged(int value)
    {
        SetRadioactivityRpc(value);
    }

    [ClientRpc]
    private void SetRadioactivityRpc(int value)
    {
        Radioactivity.Current.Value = value;
    }

    #endregion

    #region SetTarget

    [Command]
    protected void SetTargetCommand(Vector3 target)
    {
        Target = target;
        SetTargetRpc(target);
    }

    [ClientRpc]
    private void SetTargetRpc(Vector3 target)
    {
        Target = target;
    }

    #endregion

    #region Use

    [Command]
    protected void UseAbilityCommand(int index)
    {
        if (index < 0 || index >= Abilities.Length) return;

        if (!CanUseAbility(index)) return;

        UseAbilityServer(index);
        UseAbilityRpc(index);
    }

    [Server]
    private void UseAbilityServer(int index)
    {
        ((InstantAbility)Abilities[index]).ServerUse();
    }

    [ClientRpc]
    private void UseAbilityRpc(int index)
    {
        ((InstantAbility)Abilities[index]).ClientUse();
    }

    #endregion

    #region StartUsing

    [Command]
    protected void StartUsingAbilityCommand(int index)
    {
        if (index < 0 || index >= Abilities.Length) return;

        if (!CanUseAbility(index)) return;

        StartUsingAbilityServer(index);
        StartUsingAbilityRpc(index);
    }

    [Server]
    private void StartUsingAbilityServer(int index)
    {
        ((ProgressiveAbility)Abilities[index]).ServerStartUsing();
    }

    [ClientRpc]
    private void StartUsingAbilityRpc(int index)
    {
        ((ProgressiveAbility)Abilities[index]).ClientStartUsing();
    }

    #endregion

    #region StopUsing

    [Server]
    public void ForceStopUsingAbility(AbilityBase ability)
    {
        int index = Array.IndexOf(Abilities, ability);

        if (index == -1) return;

        StopUsingAbilityServer(index);
        StopUsingAbilityRpc(index);
    }

    [Command]
    protected void StopUsingAbilityCommand(int index)
    {
        if (index < 0 || index >= Abilities.Length) return;

        StopUsingAbilityServer(index);
        StopUsingAbilityRpc(index);
    }

    [Server]
    private void StopUsingAbilityServer(int index)
    {
        ((ProgressiveAbility)Abilities[index]).ServerStopUsing();
    }

    [ClientRpc]
    private void StopUsingAbilityRpc(int index)
    {
        ((ProgressiveAbility)Abilities[index]).ClientStopUsing();
    }

    #endregion

    protected virtual void Awake()
    {
        Radioactivity = new Radioactivity(OnRadioactivityChanged);
    }

    protected virtual void Update()
    {
        foreach (AbilityBase a in Abilities) {
            a.Update();
        }
    }
}