using System.Collections;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private GameObject walkerPrefab;
    [SerializeField] private GameObject hunterPrefab;
    [SerializeField] private GameObject mutantPrefab;
    [SerializeField] private float innerSpawnRadius;
    [SerializeField] private float outerSpawnRadius;

    public ListenableList<Enemy> Enemies { get; } = new();

    [Server]
    private void SpawnRandom(GameObject prefab)
    {
        GameObject newInstance = Instantiate(prefab, GetRandomSpawnPosition(), Quaternion.identity);
        NetworkServer.Spawn(newInstance);
    }

    [Server]
    public void SpawnWave(WavePattern wave, int multiplicator)
    {
        for (int i = 0; i < wave.walkers * multiplicator; i++) {
            SpawnRandom(walkerPrefab);
        }
        for (int i = 0; i < wave.hunters * multiplicator; i++) {
            SpawnRandom(hunterPrefab);
        }
        for (int i = 0; i < wave.mutants * multiplicator; i++) {
            SpawnRandom(mutantPrefab);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, innerSpawnRadius);
        Gizmos.DrawWireSphere(transform.position, outerSpawnRadius);
    }
}