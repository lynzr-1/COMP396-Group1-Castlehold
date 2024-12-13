using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class LevelConfig
{
    public List<WaveConfig> waves; // Waves specific to this level
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Level and Wave Settings")]
    public List<LevelConfig> levels = new List<LevelConfig>(); // Configurable levels with waves
    public float timeBeforeFirstWave = 5f;
    public float timeBetweenWaves = 10f;
    public float timeBetweenSpawns = 1f;

    [Header("Spawner Settings")]
    private Transform spawnPoint;

    private int currentLevelIndex = 0; // Tracks the current level
    private int waveNumber = 0; // Tracks the current wave within the level
    private int activeEnemies = 0;

    private bool isWaveInProgress = false;

    // Events for UI updates
    public event Action<float> OnCountdownStarted;
    public event Action<int> OnWaveStarted;
    public event Action OnAllWavesCompleted;

    private void Start()
    {
        Debug.Log($"Total levels configured: {levels.Count}");

        GameObject spawnPointObject = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawnPointObject != null)
        {
            spawnPoint = spawnPointObject.transform;
        }
        else
        {
            Debug.LogError("Spawn point not found");
            return;
        }

        StartLevel(0);
    }

    public void StartLevel(int levelIndex)
    {
        if (levelIndex >= levels.Count)
        {
            Debug.Log("No more levels to start.");
            return;
        }

        currentLevelIndex = levelIndex;
        waveNumber = 0; // Reset wave count for the new level
        Debug.Log($"Starting Level {currentLevelIndex + 1}");

        StartCoroutine(StartFirstWave());
    }

    private IEnumerator StartFirstWave()
    {
        OnCountdownStarted?.Invoke(timeBeforeFirstWave);

        float countdown = timeBeforeFirstWave;
        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        if (isWaveInProgress)
        {
            yield break;
        }

        var currentLevel = levels[currentLevelIndex];
        if (waveNumber >= currentLevel.waves.Count)
        {
            Debug.Log($"All waves completed for Level {currentLevelIndex + 1}");
            OnAllWavesCompleted?.Invoke();
            yield break;
        }

        isWaveInProgress = true;

        WaveConfig currentWave = currentLevel.waves[waveNumber];
        Debug.Log($"Starting Wave: {waveNumber + 1} in Level {currentLevelIndex + 1}");

        OnWaveStarted?.Invoke(waveNumber + 1);

        StartCoroutine(SpawnWave(currentWave));

        while (activeEnemies > 0)
        {
            yield return null;
        }

        Debug.Log($"Wave {waveNumber + 1} completed!");

        yield return new WaitForSeconds(timeBetweenWaves);

        waveNumber++;
        isWaveInProgress = false;

        StartCoroutine(StartNextWave());
    }

    private IEnumerator SpawnWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.enemiesToSpawn; i++)
        {
            AbstractFactory selectedFactory = SelectFactoryForWave(waveConfig.enemyFactories);
            selectedFactory.CreateEnemy();

            activeEnemies++;
            Debug.Log($"SpawnWave: Active Enemies: {activeEnemies}");

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private AbstractFactory SelectFactoryForWave(List<AbstractFactory> factories)
    {
        return factories[UnityEngine.Random.Range(0, factories.Count)];
    }

    public void OnEnemyRemoved()
    {
        activeEnemies--;
        activeEnemies = Mathf.Max(0, activeEnemies);

        Debug.Log($"OnEnemyRemoved: Active Enemies = {activeEnemies}");

        if (activeEnemies == 0 && isWaveInProgress)
        {
            Debug.Log("Wave complete!");
        }
    }

    //****HELPER METHODS****//

    public int GetWaveCountForCurrentLevel()
    {
        if (currentLevelIndex >= 0 && currentLevelIndex < levels.Count)
        {
            return levels[currentLevelIndex].waves.Count;
        }
        return 0;
    }

    public bool LevelsConfigured()
    {
        return levels != null && levels.Count > 0;
    }
}
