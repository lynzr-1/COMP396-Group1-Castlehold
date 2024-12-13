using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Wave Counter UI")]
    [SerializeField] private TextMeshProUGUI _waveCounter;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private EnemySpawner _enemySpawner; //to get the wave count

    [Header("Gold Counter UI")]
    [SerializeField] private TextMeshProUGUI _goldCounterText;

    [Header("Level Complete UI")]
    [SerializeField] private GameObject _levelCompletePanel;
    [SerializeField] private TextMeshProUGUI _levelCompleteText;

    private int _waveCount;
    private int _currentWave;

    // Start is called before the first frame update
    void Start()
    {
        // **** WAVE COUNTER **** //
        if (_enemySpawner != null && _enemySpawner.LevelsConfigured())
        {
            _waveCount = _enemySpawner.GetWaveCountForCurrentLevel(); // Fetch wave count
            _waveCounter.text = $"Wave Incoming";

            // Subscribe to spawner events
            _enemySpawner.OnWaveStarted += UpdateWaveCounter;
            _enemySpawner.OnCountdownStarted += StartCountdown;
        }
        else
        {
            Debug.LogError("EnemySpawner not configured or found!");
        }

        //subscribe to spawner events
        _enemySpawner.OnWaveStarted += UpdateWaveCounter;
        _enemySpawner.OnCountdownStarted += StartCountdown;

        // Hide Level Complete Panel
        if (_levelCompletePanel != null)
        {
            _levelCompletePanel.SetActive(false);
        }
    }

    private void UpdateWaveCounter(int currentWave)
    {
        _waveCounter.text = $"Wave {currentWave} of {_waveCount}";
    }

    private void StartCountdown(float countdownTime)
    {
        StartCoroutine(CountdownCoroutine(countdownTime));
    }

    private IEnumerator CountdownCoroutine(float countdownTime)
    {
        while (countdownTime > 0)
        {
            _countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
            countdownTime -= Time.deltaTime;
            yield return null;
        }

        _countdownText.text = ""; // Clear the countdown text
    }

    public void UpdateGoldCounter(int currentGold) 
    {
        _goldCounterText.text = $"{currentGold}";
    }

    public void ShowLevelComplete(int currentLevel, int totalLevels)
    {
        if (_levelCompletePanel != null)
        {
            _levelCompletePanel.SetActive(true);
            _levelCompleteText.text = $"Level {currentLevel} Complete!";

            if (currentLevel >= totalLevels)
            {
                _levelCompleteText.text += "\nCongratulations! You've completed all levels!";
            }
        }
    }

    public void HideLevelComplete()
    {
        if (_levelCompletePanel != null)
        {
            _levelCompletePanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        _enemySpawner.OnWaveStarted -= UpdateWaveCounter;
        _enemySpawner.OnCountdownStarted -= StartCountdown;
    }
}
