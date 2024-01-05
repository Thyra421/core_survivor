using System.Linq;
using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Player))]
public abstract class PlayerClass : NetworkBehaviour
{
    public AbilityBase[] Abilities { get; protected set; }
    public Radioactivity Radioactivity { get; private set; }
    public bool IsBusy => Abilities.Any(a => !a.IsCompleted);

    public virtual Vector3? Target {
        get {
            AbilityBase ability = Abilities.FirstOrDefault(a => !a.IsCompleted);
            return ability is ITargeted targeted ? targeted.Target : null;
        }
    }

    protected bool CanUseAbility(int index)
    {
        if (Abilities[index].IsChanneled)
            return Abilities[index].Cooldown.IsReady;
        return !IsBusy && Abilities[index].Cooldown.IsReady;
    }

    [ClientRpc]
    private void SyncRadioactivityRpc(int value)
    {
        Radioactivity.Current.Value = value;
    }

    [Command]
    protected void UseAbilityCommand(int index, string args)
    {
        if (!CanUseAbility(index)) return;

        UseAbilityRpc(index, args);
        Abilities[index].ServerUse(args);
    }

    [ClientRpc]
    private void UseAbilityRpc(int index, string args)
    {
        Abilities[index].ClientUse(args);
    }

    [Command]
    protected void EndUseAbilityCommand(int index, string args)
    {
        EndUseAbilityRpc(index, args);
        Abilities[index].ServerEnd(args);
    }

    [ClientRpc]
    private void EndUseAbilityRpc(int index, string args)
    {
        Abilities[index].ClientEnd(args);
    }

    protected virtual void Awake()
    {
        Radioactivity = new Radioactivity(SyncRadioactivityRpc);
    }

    protected virtual void Update()
    {
        foreach (AbilityBase a in Abilities) {
            a.Cooldown.Update();
        }
    }
}