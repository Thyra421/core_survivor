using System.Linq;
using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class PlayerClass : NetworkBehaviour
{
    public AbilityBase[] Abilities { get; protected set; }
    public bool IsBusy => Abilities.Any(a => !a.IsCompleted);

    public Vector3? Target {
        get {
            AbilityBase ability = Abilities.FirstOrDefault(a => !a.IsCompleted);
            return ability is ITargeted targeted ? targeted.Target : null;
        }
    }

    protected virtual bool CanUseAbility(int index)
    {
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

    private void Update()
    {
        foreach (AbilityBase a in Abilities) {
            a.Cooldown.Update();
        }
    }
}