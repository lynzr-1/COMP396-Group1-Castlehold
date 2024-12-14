using UnityEngine;
using UnityEngine.Pool;

public class SkeletonFactory : AbstractFactory
{
    public PoolManager skeletonPoolManager;  // Reference to the object pool for skeleton

    public override void CreateEnemy(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject skeleton = skeletonPoolManager.GetObject(); ///get skeleton from the pool
        skeleton.transform.position = spawnPosition;
        skeleton.transform.rotation = spawnRotation;

        //assign the pool manager reference to the enemy
        skeleton.GetComponent<EnemyBehaviour>().poolManager = skeletonPoolManager;

        // Set destination
        if (TargetLocation != null)
        {
            skeleton.GetComponent<SkeletonAgent>().Navigate(TargetLocation.position);
        }
        else
        {
            Debug.LogError("[SkeletonFactory] TargetLocation is null - cannot navigate.");
        }
    }
    private void Start()
    {
        if (TargetLocation == null)
        {
            TargetLocation = GameObject.FindWithTag("EnemyPathEnd")?.transform;
            if (TargetLocation == null)
            {
                Debug.LogError($"[Factory] TargetLocation not found for {gameObject.name}.");
            }
        }
    }
}