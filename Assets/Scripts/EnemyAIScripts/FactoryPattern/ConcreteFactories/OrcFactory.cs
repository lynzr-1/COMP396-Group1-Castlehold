using UnityEngine;
using UnityEngine.Pool;

public class OrcFactory : AbstractFactory
{
    public PoolManager orcPoolManager;  // Reference to the object pool for orcs

    public override void CreateEnemy()
    {
        GameObject orc = orcPoolManager.GetObject(); ///get orc from the pool
        orc.transform.position = SpawnLocation.position;
        orc.transform.rotation = SpawnLocation.rotation;

        //assign the pool manager reference to the enemy
        orc.GetComponent<EnemyBehaviour>().poolManager = orcPoolManager;

        //set destination for the orc
        orc.GetComponent<OrcAgent>().Navigate(TargetLocation.position);
    }
}