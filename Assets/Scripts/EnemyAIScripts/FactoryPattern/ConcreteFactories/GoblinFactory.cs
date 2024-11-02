using UnityEngine;
using UnityEngine.Pool;

public class GoblinFactory : AbstractFactory
{
    public PoolManager goblinPoolManager;  // Reference to the object pool for goblins
    public override void CreateEnemy()
    {
        GameObject goblin = goblinPoolManager.GetObject(); ///get goblin from the pool
        goblin.transform.position = SpawnLocation.position;
        goblin.transform.rotation = SpawnLocation.rotation;

        //assign the pool manager reference to the enemy
        goblin.GetComponent<EnemyBehaviour>().poolManager = goblinPoolManager;

        //set destination for the orc
        goblin.GetComponent<GoblinAgent>().Navigate(TargetLocation.position);
    }
}