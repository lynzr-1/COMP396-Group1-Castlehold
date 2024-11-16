using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldCounterText;
    [SerializeField] private TextMeshProUGUI _waveCounter;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private EnemySpawner _enemySpawner; //to get the wave count

    private int _waveCount;
    private int _currentWave;

    // Start is called before the first frame update
    void Start()
    {
        //**** GOLD COUNTER ****//
        _goldCounterText.text = "100"; //replace with variables from gold counter script later

        //**** WAVE COUNTER ****//
        _waveCount = _enemySpawner.waves.Count; //get the total number of waves
        _waveCounter.text = $"Wave Incoming";

        // Subscribe to spawner events
        _enemySpawner.OnWaveStarted += UpdateWaveCounter;
        _enemySpawner.OnCountdownStarted += StartCountdown;
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

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        _enemySpawner.OnWaveStarted -= UpdateWaveCounter;
        _enemySpawner.OnCountdownStarted -= StartCountdown;
    }
}
