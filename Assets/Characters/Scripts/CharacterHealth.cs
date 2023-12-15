using Mirror;

public abstract partial class CharacterHealth : NetworkBehaviour, IDamageable
{
    [SyncVar(hook = nameof(CurrentHealthHook))]
    private int _currentHealth;

    public readonly Listenable<int> current = new();

    private void CurrentHealthHook(int oldValue, int newValue)
    {
        _currentHealth = newValue;
        current.Value = newValue;
    }

    [SyncVar(hook = nameof(MaxHealthHook))]
    private int _maxHealth = 100;

    public readonly Listenable<int> max = new(100);

    private void MaxHealthHook(int oldValue, int newValue)
    {
        _maxHealth = newValue;
        max.Value = newValue;
    }
}