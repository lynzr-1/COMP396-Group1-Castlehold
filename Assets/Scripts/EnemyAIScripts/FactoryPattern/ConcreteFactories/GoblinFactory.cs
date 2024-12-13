using UnityEngine;
using UnityEngine.Pool;

public class GoblinFactory : AbstractFactory
{
    public PoolManager goblinPoolManager;  // Reference to the object pool for goblins
    public override void CreateEnemy(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject goblin = goblinPoolManager.GetObject(); ///get goblin from the pool
        goblin.transform.position = SpawnLocation.position;
        goblin.transform.rotation = SpawnLocation.rotation;

        //assign the pool manager reference to the enemy
        goblin.GetComponent<EnemyBehaviour>().poolManager = goblinPoolManager;

        // Set destination
        if (TargetLocation != null)
        {
            goblin.GetComponent<GoblinAgent>().Navigate(TargetLocation.position);
        }
        else
        {
            Debug.LogError("[GoblinFactory] TargetLocation is null - cannot navigate.");
        }
    }
}