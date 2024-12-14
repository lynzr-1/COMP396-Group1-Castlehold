using UnityEngine;

public abstract class AbstractFactory : MonoBehaviour
{
    //public List<GameObject> EnemyPrefabs;  // Enemy types for this factory
    public Transform TargetLocation;  // Where enemies will move toward (e.g., castle)

    public abstract void CreateEnemy(Vector3 spawnPosition, Quaternion spawnRotation);
}
