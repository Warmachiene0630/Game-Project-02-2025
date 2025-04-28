using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to allow multiple types // Assign Enemy1.prefab in Inspector
    public Transform[] spawnPoints; // Array of spawn locations
    public float spawnInterval = 3f; // Time between spawns
    public int maxEnemies = 9; // Limit to avoid performance issues

    private int currentEnemyCount = 0;
    private readonly List<GameObject> activeEnemies = new(); // Track spawned enemies

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 || enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);

        currentEnemyCount++;
        activeEnemies.Add(spawnedEnemy);

        GameManager.instance.updateGameGoal(1); // Register enemy in game goal

        spawnedEnemy.AddComponent<EnemyDeathHandler>().Initialize(this);
    }

    public void EnemyDied(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        currentEnemyCount--;
        GameManager.instance.updateGameGoal(-1);
    }
}

public class EnemyDeathHandler : MonoBehaviour
{
    private MobSpawner spawner;

    public void Initialize(MobSpawner mobSpawner)
    {
        spawner = mobSpawner;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.EnemyDied(gameObject);
        }
    }
}
