using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        if (_attackTimer <= 0f && enemiesInRange.Count > 0)
        {
            Attack();
            _attackTimer = attackInterval;
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
        //if the enemy leaves the tower's range, remove it from the list
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Attack()
    {
        if (enemiesInRange.Count > 0)
        {
            GameObject targetEnemy = enemiesInRange[0]; //attack the first enemy in range - we can change this logic later if needed

            if (targetEnemy != null) //make sure the enemy still exists in game
            {
                FireProjectile(targetEnemy);

                //play any applicable attack effects here
                PlaySmokeEffect();

                Debug.Log($"Attacked {targetEnemy.name} for {damage} damage.");
            }
            else
            {
                enemiesInRange.RemoveAt(0); // Remove the destroyed object from the list
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
}
