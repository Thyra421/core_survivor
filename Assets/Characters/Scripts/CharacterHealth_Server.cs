using System;
using Mirror;

public abstract partial class CharacterHealth
{
    public override void OnStartServer()
    {
        _currentHealthSync = _maxHealthSync;
        max.OnValueChanged += i =>
        {
            if (i < current.Value)
                _currentHealthSync = i;
        };
    }

    [Server]
    public void TakeDamage(int amount)
    {
        _currentHealthSync = Math.Clamp(_currentHealthSync - amount, 0, _maxHealthSync);

        if (_currentHealthSync == 0)
            Die();
    }

    public abstract void Die();
}