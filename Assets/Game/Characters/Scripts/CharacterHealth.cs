using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public abstract partial class CharacterHealth : NetworkBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth;

    [SyncVar(hook = nameof(CurrentHealthHook))]
    private int _currentHealthSync;

    public int Max => maxHealth;
    public Listenable<int> Current { get; } = new();
    public bool IsFullHealth => Current.Value == maxHealth;
    public bool IsDead => Current.Value == 0;

    private void CurrentHealthHook(int oldValue, int newValue)
    {
        _currentHealthSync = newValue;
        Current.Value = newValue;
    }
}