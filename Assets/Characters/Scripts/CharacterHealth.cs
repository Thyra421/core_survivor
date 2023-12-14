using Mirror;

public abstract partial class CharacterHealth : NetworkBehaviour, IDamageable
{
    [SyncVar(hook = nameof(CurrentHealthHook))]
    private int _currentHealth;

    [SyncVar(hook = nameof(MaxHealthHook))]
    private int _maxHealth = 100;

    private void CurrentHealthHook(int oldValue, int newValue) => _currentHealth = newValue;
    private void MaxHealthHook(int oldValue, int newValue) => _maxHealth = newValue;
}