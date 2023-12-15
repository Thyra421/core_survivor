using Mirror;

public partial class Enemy : NetworkBehaviour
{
    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
    
    private void Start()
    {
        EnemyManager.Current.Enemies.Add(this);
    }

    private void OnDestroy()
    {
        EnemyManager.Current.Enemies.Remove(this);
    }
}