using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public int[] enemiesPerWave; //set these values in the inspector to set the number of enemies to be spawned in each wave

    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Spawner Settings")]
    private Transform spawnPoint;
    public float timeBetweenWaves = 10f;
    public float timeBetweenSpawns = 1f;
    private int waveNumber = 0;
    private bool isSpawning = false;

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
        yield return new WaitForSeconds(timeBetweenWaves); //pause between waves

        //start spawning the wave if wave number is valid
        if (waveNumber < enemiesPerWave.Length)
        {
            int enemiesToSpawn = enemiesPerWave[waveNumber];
            StartCoroutine(SpawnWave(enemiesToSpawn));
            waveNumber++;
        }
        else
        {
            Debug.Log("All waves completed!");
            //create logic to call Level Complete screen here and transition to the next level
            //if it is level 3 being completed, call the game over logic instead
        }
    }

    private IEnumerator SpawnWave(int enemiesToSpawn)
    {

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            //select enemy type based on wave progression
            GameObject enemyPrefab = SelectEnemyForWave();
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);  //delay between enemy spawns within the wave
        }

        StartCoroutine(StartNextWave());  // Start the next wave after this one completes
        Debug.Log("Starting next wave");
    }

    private GameObject SelectEnemyForWave()
    {
        // Example: Randomly select an enemy from the array
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }
}
