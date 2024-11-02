using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveConfig
{
    public int enemiesToSpawn;  // Number of enemies in this wave
    public List<AbstractFactory> enemyFactories;  // List of factories for this wave
}
