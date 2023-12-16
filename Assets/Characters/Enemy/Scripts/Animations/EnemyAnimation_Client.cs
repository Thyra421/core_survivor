using Mirror;

public partial class EnemyAnimation
{
    [Client]
    public void ClientSetTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
}