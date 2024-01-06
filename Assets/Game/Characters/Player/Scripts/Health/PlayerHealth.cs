using Mirror;

public class PlayerHealth : CharacterHealth
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    [Server]
    public override void Die()
    {
        DieRpc();
        PlayerManager.Current.PlayersAlive.Value--;
        PlayerManager.Current.SetPlayersAliveRpc(PlayerManager.Current.PlayersAlive.Value);
    }

    [ClientRpc]
    private void DieRpc()
    {
        player.Animation.SetTrigger("Die");
    }
}