using Mirror;

public partial class EnemyAnimation
{
    [Client]
    public void SetTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
}