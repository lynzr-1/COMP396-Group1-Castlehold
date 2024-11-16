using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackInterval = 1.0f; //time between attacks
    public float damage = 5f; //amount of damage this tower does
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    [Header("Tower Visual Effects")]
    public GameObject smokePrefab;
    public Transform smokeSpawner;

    private float _attackTimer = 0f; //to manage attack intervals
    private GameObject _target;

    private List<GameObject> enemiesInRange = new List<GameObject>(); //list to track enemies in range of this tower

    private void Update()
    {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0f)
        {
            CleanupEnemiesList();

            if (enemiesInRange.Count > 0)
            {
                Attack();
                _attackTimer = attackInterval;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //find nearby enemies and add them to the enemy in range list
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            Debug.Log("Added enemy to list");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Remove(other.gameObject);
                Debug.Log($"Removed {other.gameObject.name} from enemiesInRange.");
            }
            else
            {
                Debug.LogWarning($"Enemy {other.gameObject.name} was not found in enemiesInRange!");
            }
        }
    }

    private void Attack()
    {
        if (enemiesInRange.Count > 0)
        {
            GameObject targetEnemy = enemiesInRange[0]; //attack the first enemy in range - we can change this logic later if needed

            if (IsInRange(targetEnemy))
            {
                FireProjectile(targetEnemy);
                PlaySmokeEffect();
                Debug.Log($"Attacked {targetEnemy.name} for {damage} damage.");
            }
            else
            {
                enemiesInRange.Remove(targetEnemy); // Remove enemy if it's out of range
                Debug.LogWarning($"{targetEnemy.name} is out of range and was removed.");
            }
        }
    }

    private void FireProjectile(GameObject targetEnemy)
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Set the projectile's target and pass the damage
            Projectiles projectileScript = projectile.GetComponent<Projectiles>();

            if (projectileScript != null)
            {
                projectileScript.SetTarget(targetEnemy.transform);
                projectileScript.damage = damage; // Pass the tower's damage value to the projectile
            }
        }
    }

    private void PlaySmokeEffect()
    {
        if (smokePrefab != null && smokeSpawner != null)
        { 
            GameObject smoke = Instantiate(smokePrefab, smokeSpawner.position, smokeSpawner.rotation);
            Destroy(smoke, 2f);
        }
    }

    //**** HELPER METHODS ****//
    private bool IsInRange(GameObject enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        float range = GetComponent<SphereCollider>().radius;
        return distance <= range;
    }

    private void CleanupEnemiesList()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null || !IsInRange(enemy));
    }

    public void NotifyEnemyDestroyed(GameObject enemy)
    {
        // Remove the destroyed enemy if it's still in the list
        if (enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
            Debug.Log($"Notified: Removed destroyed enemy {enemy.name} from list.");
        }
    }
}
