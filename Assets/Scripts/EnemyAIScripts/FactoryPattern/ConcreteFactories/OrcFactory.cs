using UnityEngine;
using UnityEngine.Pool;

public class OrcFactory : AbstractFactory
{
    public PoolManager orcPoolManager;  // Reference to the object pool for orcs

    public override void CreateEnemy(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject orc = orcPoolManager.GetObject(); // Get orc from the pool
        orc.transform.position = spawnPosition;     // Use passed spawn position
        orc.transform.rotation = spawnRotation;     // Use passed spawn rotation

        Debug.Log($"[OrcFactory] Created enemy at {spawnPosition}");

        orc.GetComponent<EnemyBehaviour>().poolManager = orcPoolManager;
        orc.GetComponent<OrcAgent>().Navigate(TargetLocation.position);
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