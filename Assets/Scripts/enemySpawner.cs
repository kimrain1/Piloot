using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemiesPerWave = 5;   // Number of enemies per wave
    public float spawnDistance = 20f;
    public float spawnRate = 2f;     // Time between each enemy spawn
    public float waveDelay = 5f;     // Time between waves

    private int enemiesSpawned = 0;
    private bool spawning = false;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            spawning = true;
            enemiesSpawned = 0;

            while (enemiesSpawned < enemiesPerWave)
            {
                SpawnEnemy();
                enemiesSpawned++;
                yield return new WaitForSeconds(spawnRate);
            }

            spawning = false;
            yield return new WaitForSeconds(waveDelay); // Wait before next wave
        }
    }

    void SpawnEnemy()
    {
        Vector3 offset = Random.onUnitSphere;
        offset.z = 0;
        offset = offset.normalized * spawnDistance;

        GameObject enemy = Instantiate(enemyPrefab, transform.position + offset, Quaternion.identity);
        enemy.transform.parent = null;
    }
}
