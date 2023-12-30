using System;
using Mirror;

public class DestructibleEnvironment : CharacterHealth
{
    [Server]
    public override void Die()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void Start()
    {
        EnvironmentManager.Current.Environments.Add(this);
    }

    private void OnDestroy()
    {
        EnvironmentManager.Current.Environments.Remove(this);
    }
}
