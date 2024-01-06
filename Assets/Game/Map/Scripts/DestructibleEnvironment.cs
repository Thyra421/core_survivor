using Mirror;
using UnityEngine;

public class DestructibleEnvironment : CharacterHealth
{
    [SerializeField]
    private GameObject healthUIPrefab;

    [SerializeField]
    private bool isCore;

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
        if (isCore)
            GameManager.Current.GameOver("Le noyau a été détruit !");
        EnvironmentManager.Current.Environments.Remove(this);
    }
}