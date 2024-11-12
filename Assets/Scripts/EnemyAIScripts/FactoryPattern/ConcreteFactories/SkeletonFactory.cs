using UnityEngine;
using UnityEngine.Pool;

public class SkeletonFactory : AbstractFactory
{
    public PoolManager skeletonPoolManager;  // Reference to the object pool for skeleton

    public override void CreateEnemy()
    {
        GameObject skeleton = skeletonPoolManager.GetObject(); ///get skeleton from the pool
        skeleton.transform.position = SpawnLocation.position;
        skeleton.transform.rotation = SpawnLocation.rotation;

        //assign the pool manager reference to the enemy
        skeleton.GetComponent<EnemyBehaviour>().poolManager = skeletonPoolManager;

        //set destination for the orc
        skeleton.GetComponent<SkeletonAgent>().Navigate(TargetLocation.position);
    }
}