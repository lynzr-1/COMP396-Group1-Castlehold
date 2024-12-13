using UnityEngine;
using UnityEngine.Pool;

public class OrcFactory : AbstractFactory
{
    public PoolManager orcPoolManager;  // Reference to the object pool for orcs

    public override void CreateEnemy(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject orc = orcPoolManager.GetObject(); //get orc from the pool
        orc.transform.position = spawnPosition;     //set spawn position
        orc.transform.rotation = spawnRotation;     //set spawn rotation
        Debug.Log($"[OrcFactory] Setting spawn point at {spawnPosition} position and {spawnRotation} rotation");

        // Assign the pool manager reference to the enemy
        orc.GetComponent<EnemyBehaviour>().poolManager = orcPoolManager;

        // Set destination for the orc
        if (TargetLocation != null)
        {
            orc.GetComponent<OrcAgent>().Navigate(TargetLocation.position);
        }
        else
        {
            Debug.LogError("[OrcFactory] TargetLocation is null. Orc cannot navigate.");
        }
    }
}