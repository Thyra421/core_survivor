using Mirror;

public partial class PlayerAnimation
{
    [Client]
    public void SetTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
}