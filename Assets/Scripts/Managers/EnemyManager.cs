using Mirror;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float innerSpawnRadius;
    [SerializeField] private float outerSpawnRadius;

    public ListenableList<Enemy> Enemies { get; } = new();

    [Server]
    public void SpawnRandom()
    {
        GameObject newInstance = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        NetworkServer.Spawn(newInstance);
    }
    
    [Server]
    public void SpawnWave(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnRandom();
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomRadius = Random.Range(innerSpawnRadius, outerSpawnRadius);
        float x = randomRadius * Mathf.Cos(randomAngle);
        float z = randomRadius * Mathf.Sin(randomAngle);
        Vector3 randomPosition = new Vector3(x, 0f, z) + transform.position;

        return randomPosition;
    }
}