using Mirror;

public abstract partial class CharacterHealth : NetworkBehaviour, IDamageable
{
    [SyncVar(hook = nameof(CurrentHealthHook))]
    private int _currentHealthSync;
    
    public bool IsFullHealth => current.Value == max.Value;
    public bool IsDead => current.Value == 0;

    public readonly Listenable<int> current = new();

    private void CurrentHealthHook(int oldValue, int newValue)
    {
        _currentHealthSync = newValue;
        current.Value = newValue;
    }

    [SyncVar(hook = nameof(MaxHealthHook))]
    private int _maxHealthSync = 100;

    public readonly Listenable<int> max = new(100);

    private void MaxHealthHook(int oldValue, int newValue)
    {
        _maxHealthSync = newValue;
        max.Value = newValue;
    }
}