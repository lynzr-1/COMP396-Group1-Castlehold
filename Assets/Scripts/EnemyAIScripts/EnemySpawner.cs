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
        Debug.Log($"Total waves configured: {waves.Count}");

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
            yield break;  //prevent multiple calls or starting waves beyond the total count
        }

        isWaveInProgress = true;

        //spawn the current wave
        WaveConfig currentWave = waves[waveNumber];
        Debug.Log($"Starting Wave: {waveNumber}");

        OnWaveStarted?.Invoke(waveNumber +1);

        StartCoroutine(SpawnWave(currentWave));

        //wait for all enemies in the wave to be cleared
        while (activeEnemies > 0)
        {
            yield return null;
        }

        Debug.Log($"Wave {waveNumber} completed!");

        //wait between waves
        yield return new WaitForSeconds(timeBetweenWaves);

        waveNumber++;

        Debug.Log($"StartNextWave: Increment waveNumber: {waveNumber}");

        //start the next wave

        isWaveInProgress = false;

        if (waveNumber < waves.Count)
        {
            Debug.Log($"Wave number: {waveNumber}, Waves.count: {waves.Count} for if statement");
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
            Debug.Log($"SpawnWave: Active Enemies: {activeEnemies}");

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
        Debug.Log($"OnEnemyRemoved: Active Enemies before decrement: {activeEnemies}");
        activeEnemies--;
        Debug.Log($"OnEnemyRemoved: Active enemies after decrement = {activeEnemies}");
        activeEnemies = Mathf.Max(0, activeEnemies);  //so count doesn't go negative

        //check if wave is complete
        if (activeEnemies == 0 && isWaveInProgress)
        {
            Debug.Log("Wave complete!");
        }
    }
}
