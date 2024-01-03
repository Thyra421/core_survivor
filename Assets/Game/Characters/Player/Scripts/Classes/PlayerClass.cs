using System.Linq;
using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Player))]
public abstract class PlayerClass : NetworkBehaviour
{
    public AbilityBase[] Abilities { get; protected set; }
    public bool IsBusy => Abilities.Any(a => !a.IsCompleted);

    public virtual Vector3? Target {
        get {
            AbilityBase ability = Abilities.FirstOrDefault(a => !a.IsCompleted);
            return ability is ITargeted targeted ? targeted.Target : null;
        }
    }

    protected virtual bool CanUseAbility(int index)
    {
        if (Abilities[index].IsChanneled)
            return Abilities[index].Cooldown.IsReady;
        return !IsBusy && Abilities[index].Cooldown.IsReady;
    }

    [Command]
    protected void UseAbilityCommand(int index, string args)
    {
        if (!CanUseAbility(index)) return;

        UseAbilityClient(index, args);
        UseAbilityServer(index, args);
    }

    [ClientRpc]
    private void UseAbilityClient(int index, string args)
    {
        Abilities[index].ClientUse(args);
    }

    [Server]
    private void UseAbilityServer(int index, string args)
    {
        Abilities[index].ServerUse(args);
    }

    [Command]
    protected void EndUseAbilityCommand(int index, string args)
    {
        EndUseAbilityServer(index, args);
        EndUseAbilityClient(index, args);
    }

    [ClientRpc]
    private void EndUseAbilityClient(int index, string args)
    {
        Abilities[index].ClientEnd(args);
    }

    [Server]
    private void EndUseAbilityServer(int index, string args)
    {
        Abilities[index].ClientEnd(args);
    }

    protected virtual void Update()
    {
        foreach (AbilityBase a in Abilities) {
            a.Cooldown.Update();
        }
    }
}