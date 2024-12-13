using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public abstract class AbstractFactory : MonoBehaviour
{
    //public List<GameObject> EnemyPrefabs;  // Enemy types for this factory
    public Transform SpawnLocation;  // Where to spawn enemies
    public Transform TargetLocation;  // Where enemies will move toward (e.g., castle)

    public abstract void CreateEnemy(Vector3 spawnPosition, Quaternion spawnRotation);
}
