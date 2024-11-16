using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<WaveConfig> waves = new List<WaveConfig>();
    public float timeBeforeFirstWave = 5f;
    public float timeBetweenWaves = 10f;
    public float timeBetweenSpawns = 1f;

    [Header("Spawner Settings")]
    private Transform spawnPoint;

    public int waveNumber = 0; //this is set automatically
    private int activeEnemies = 0;

    private bool isWaveInProgress = false; //to ensure one wave completes before the next begins
    
    //to notify the UI manager about updates
    public event Action<float> OnCountdownStarted;
    public event Action<int> OnWaveStarted;

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
            return;
        }

        StartCoroutine(StartFirstWave());
    }

    private IEnumerator StartFirstWave()
    {
        // Notify the UIManager about the countdown
        OnCountdownStarted?.Invoke(timeBeforeFirstWave);

        float countdown = timeBeforeFirstWave;

        // Show countdown on the HUD
        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        if (isWaveInProgress || waveNumber >= waves.Count)
        {
            yield break;  // Prevent multiple calls or starting waves beyond the total count
        }

        waveNumber++;
        OnWaveStarted?.Invoke(waveNumber);

        isWaveInProgress = true;

        // Spawn the current wave
        WaveConfig currentWave = waves[waveNumber];
        StartCoroutine(SpawnWave(currentWave));

        // Wait for all enemies in the wave to be cleared
        while (activeEnemies > 0)
        {
            yield return null;
        }

        // Wait between waves
        yield return new WaitForSeconds(timeBetweenWaves);

        // Proceed to the next wave

        isWaveInProgress = false;

        if (waveNumber < waves.Count)
        {
            StartCoroutine(StartNextWave());
        }
        else
        {
            Debug.Log("All waves completed!");
            // Trigger end-of-level logic here
        }
    }

    private IEnumerator SpawnWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.enemiesToSpawn; i++)
        {
            AbstractFactory selectedFactory = SelectFactoryForWave(waveConfig.enemyFactories);
            selectedFactory.CreateEnemy();
            activeEnemies++;  // Increment active enemies
            yield return new WaitForSeconds(timeBetweenSpawns);  //add specified delay between enemy spawns
        }
    }

    private AbstractFactory SelectFactoryForWave(List<AbstractFactory> factories)
    {
        return factories[UnityEngine.Random.Range(0, factories.Count)];
    }

    //call this function when an enemy dies or reaches the castle
    public void OnEnemyRemoved()
    {
        activeEnemies--;
        activeEnemies = Mathf.Max(0, activeEnemies);  //ensure it doesn't go negative
    }
}
