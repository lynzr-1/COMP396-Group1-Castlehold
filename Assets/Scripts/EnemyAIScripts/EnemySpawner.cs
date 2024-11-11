using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<WaveConfig> waves = new List<WaveConfig>();  // List of wave configurations

    [Header("Spawner Settings")]
    private Transform spawnPoint;
    public float timeBetweenWaves = 10f;
    public float timeBetweenSpawns = 1f;
    private int waveNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnPointObject = GameObject.FindGameObjectWithTag("SpawnPoint"); //get the spawn point object
        
        //if the spawn point exists, get its transform
        if (spawnPointObject != null)
        {
            spawnPoint = spawnPointObject.transform;
        }
        else
        {
            Debug.Log("Spawn point not found");
        }

        StartCoroutine(StartNextWave());
        Debug.Log("Starting first wave");
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);  // Pause between waves

        if (waveNumber < waves.Count)
        {
            WaveConfig currentWave = waves[waveNumber];
            StartCoroutine(SpawnWave(currentWave));
            waveNumber++;
        }
        else
        {
            Debug.Log("All waves completed!");
            // Handle end of level or transition to the next level
        }
    }

    private IEnumerator SpawnWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.enemiesToSpawn; i++)
        {
            AbstractFactory selectedFactory = SelectFactoryForWave(waveConfig.enemyFactories);
            selectedFactory.CreateEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);  // Delay between enemy spawns within the wave
        }

        StartCoroutine(StartNextWave());  // Start the next wave after this one completes
        Debug.Log("Starting next wave");
    }

    private AbstractFactory SelectFactoryForWave(List<AbstractFactory> factories)
    {
        // Randomly select a factory for variety in enemy types
        return factories[Random.Range(0, factories.Count)];
    }
}
