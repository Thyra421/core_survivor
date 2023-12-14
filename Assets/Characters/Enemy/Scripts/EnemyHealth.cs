using Mirror;

public class EnemyHealth : CharacterHealth
{
    [Server]
    public override void Die()
    {
        NetworkServer.Destroy(gameObject);
    }
}