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

    private int currentLevelIndex = 0; //tracks the current level
    private int waveNumber = 0; //tracks the current wave within the level
    private int activeEnemies = 0;

    private bool isWaveInProgress = false; //flag for wave completion
    private bool isLevelComplete = false; //flag for level completion

    // Events for UI updates
    public event Action<float> OnCountdownStarted;
    public event Action<int> OnWaveStarted;
    public event Action OnAllWavesCompleted;

    private void Start()
    {
        Debug.Log($"[EnemySpawner] Total levels configured: {levels.Count}");

        GameObject spawnPointObject = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawnPointObject != null)
        {
            spawnPoint = spawnPointObject.transform;
        }
        else
        {
            Debug.LogError("[EnemySpawner] Spawn point not found");
            return;
        }

        StartLevel(0);
    }

    public void StartLevel(int levelIndex)
    {
        if (levelIndex >= levels.Count)
        {
            Debug.Log("[EnemySpawner] No more levels to start.");
            return;
        }

        currentLevelIndex = levelIndex;
        waveNumber = 0; //reset wave count for the new level
        isLevelComplete = false;
        Debug.Log($"[EnemySpawner] Starting Level {currentLevelIndex + 1}");

        StartCoroutine(StartFirstWave());
    }

    private IEnumerator StartFirstWave()
    {
        Debug.Log($"[EnemySpawner] Countdown before first wave: {timeBeforeFirstWave} seconds");
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
        if (isWaveInProgress || isLevelComplete)
        {
            Debug.Log("[EnemySpawner] Cannot start next wave - either wave is in progress or level is complete.");
            yield break;
        }

        var currentLevel = levels[currentLevelIndex];

        if (waveNumber >= currentLevel.waves.Count)
        {

            Debug.Log($"[EnemySpawner] All waves completed for Level {currentLevelIndex + 1}");
            yield return new WaitUntil(() => activeEnemies == 0);

            if (!isLevelComplete)
            {
                isLevelComplete = true;
                Debug.Log($"[EnemySpawner] All waves completed for Level {currentLevelIndex + 1}. Triggering level complete.");
                OnAllWavesCompleted?.Invoke(); //trigger level completion
            }
            yield break;
        }

        isWaveInProgress = true;

        WaveConfig currentWave = currentLevel.waves[waveNumber];
        Debug.Log($"[EnemySpawner] Starting Wave {waveNumber + 1} in Level {currentLevelIndex + 1}");
        Debug.Log($"[EnemySpawner] Enemies to spawn in this wave: {currentWave.enemiesToSpawn}");

        OnWaveStarted?.Invoke(waveNumber + 1);

        yield return StartCoroutine(SpawnWave(currentWave));

        yield return new WaitUntil(() => activeEnemies == 0);

        Debug.Log($"[EnemySpawner] Wave {waveNumber + 1} completed!");
        waveNumber++;
        isWaveInProgress = false;

        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(StartNextWave());
    }

    private IEnumerator SpawnWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.enemiesToSpawn; i++)
        {
            AbstractFactory selectedFactory = SelectFactoryForWave(waveConfig.enemyFactories);
            selectedFactory.CreateEnemy();

            activeEnemies++;
            Debug.Log($"[EnemySpawner] Spawned enemy {i + 1}/{waveConfig.enemiesToSpawn}. Active Enemies: {activeEnemies}");

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

        Debug.Log($"[EnemySpawner] Enemy removed. Active Enemies = {activeEnemies}");

        if (activeEnemies == 0 && isWaveInProgress)
        {
            Debug.Log("[EnemySpawner] Wave complete!");
            isWaveInProgress = false;
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

    public void PrepareForNextLevel()
    {
        isLevelComplete = false;
        Debug.Log("[EnemySpawner] Ready for next level.");
    }
}
