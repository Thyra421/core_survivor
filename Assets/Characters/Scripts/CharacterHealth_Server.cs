using System;
using Mirror;

public abstract partial class CharacterHealth
{
    public override void OnStartServer()
    {
        _currentHealth = _maxHealth;
        max.OnValueChanged += i =>
        {
            if (i < current.Value)
                _currentHealth = i;
        };
    }

    [Server]
    public void TakeDamage(int amount)
    {
        _currentHealth = Math.Clamp(_currentHealth - amount, 0, _maxHealth);

        if (_currentHealth == 0)
            Die();
    }

    public abstract void Die();
}