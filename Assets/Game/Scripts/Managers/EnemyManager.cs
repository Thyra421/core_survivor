using System;
using System.Collections;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float innerSpawnRadius;
    [SerializeField] private float outerSpawnRadius;

    public ListenableList<Enemy> Enemies { get; } = new();

    [Server]
    private void SpawnRandom()
    {
        GameObject newInstance = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        NetworkServer.Spawn(newInstance);
    }

    [Server]
    public IEnumerator SpawnWave(int amount)
    {
        for (int i = 0; i < amount; i++) {
            SpawnRandom();
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
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