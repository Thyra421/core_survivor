using System;
using Mirror;

public abstract partial class CharacterHealth
{
    public override void OnStartServer()
    {
        _currentHealthSync = maxHealth;
        Current.Value = maxHealth;
    }

    [Server]
    public void TakeDamage(int amount)
    {
        _currentHealthSync = Math.Clamp(_currentHealthSync - amount, 0, maxHealth);
        Current.Value = _currentHealthSync;

        if (_currentHealthSync == 0)
            Die();
    }

    public abstract void Die();
}