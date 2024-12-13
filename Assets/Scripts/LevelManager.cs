using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;
    public int currentLevel = 1;

    [Header("Level Settings")]
    public int totalLevels = 3;

    [Header("Player Stats")] //to track data for the level complete UI
    public int wavesSurvived = 0;
    public int enemiesKilled = 0;
    public int totalGoldEarned = 0;

    private EnemySpawner enemySpawner;
    private UIManager uiManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        uiManager = FindObjectOfType<UIManager>();

        if (enemySpawner != null)
        {
            enemySpawner.OnAllWavesCompleted += HandleLevelComplete;
        }
        else
        {
            Debug.LogError("EnemySpawner not found!");
        }

        StartLevel();
    }

    public void StartLevel()
    {
        Debug.Log($"Starting Level {currentLevel}");

        //reset variables for new level
        wavesSurvived = 0; 
        enemiesKilled = 0; 

        if (enemySpawner != null)
        {
            enemySpawner.StartLevel(currentLevel - 1);
        }
    }
     
    private void HandleLevelComplete()
    {
        Debug.Log($"[LevelManager] Level {currentLevel} Complete!");

        if (enemySpawner != null)
        {
            enemySpawner.PrepareForNextLevel(); //reset the spawner for the next level
        }
        
        uiManager?.ShowLevelComplete(wavesSurvived, enemiesKilled, totalGoldEarned);
    }

    //waves survived tracker method
    private void UpdateWavesSurvived()
    {
        wavesSurvived++;
    }

    //enemies killed tracker method
    public void AddToEnemiesKilled()
    {
        enemiesKilled++;
    }

    //gold earned tracker method
    public void AddToGoldEarned(int gold)
    {
        totalGoldEarned += gold;
    }

    public void LoadNextLevel()
    {
        if (currentLevel < totalLevels)
        {
            currentLevel++;
            StartLevel();
        }
        else
        {
            Debug.Log("All levels completed! Returning to main menu...");
            LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        // Replace with your main menu scene name
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}