using Mirror;
using UnityEngine;

public class DestructibleEnvironment : CharacterHealth
{
    [SerializeField]
    private GameObject healthUIPrefab;

    public override void OnStartClient()
    {
        Instantiate(healthUIPrefab, transform);
    }

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