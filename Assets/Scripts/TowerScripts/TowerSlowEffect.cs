using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlowEffect : MonoBehaviour
{
    [Header("Tower Settings")]
    public float slowEffectDuration = 3.0f; // Time the slow effect lasts after the enemy leaves the range
    public float attackInterval = 5.0f; // Time between each AoE slow effect
    public float slowMultiplier = 0.5f; // Speed reduction multiplier (e.g., 0.5 = 50% slower)
    public AudioSource audioSource;

    [Header("Tower Visual Effects")]
    public GameObject aoeEffectPrefab; // Visual AoE effect to show when the tower is active
    public Transform aoeEffectPosition;

    private float _attackTimer = 0f; // Timer to manage attack intervals
    private List<GameObject> enemiesInRange = new List<GameObject>(); // List to track enemies in range

    private void Update()
    {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0f && enemiesInRange.Count > 0)
        {
            ApplySlowEffect();
            _attackTimer = attackInterval; // Reset the timer
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            Debug.Log($"Enemy {other.name} entered slow range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Remove(other.gameObject);
                Debug.Log($"Enemy {other.name} exited slow range.");
                // Apply lingering slow effect
                EnemyBehaviour enemyBehaviour = other.GetComponent<EnemyBehaviour>();
                if (enemyBehaviour != null)
                {
                    StartCoroutine(ApplyLingeringSlow(enemyBehaviour));
                }
            }
        }
    }

    private void ApplySlowEffect()
    {
        // Apply slow to all enemies in range
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
                if (enemyBehaviour != null)
                {
                    enemyBehaviour.SetSpeedMultiplier(slowMultiplier);
                    Debug.Log($"Applied slow effect to {enemy.name}.");
                }
            }
        }

        // Trigger visual effect
        PlayAoEEffect();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private IEnumerator ApplyLingeringSlow(EnemyBehaviour enemy)
    {
        enemy.SetSpeedMultiplier(slowMultiplier);
        yield return new WaitForSeconds(slowEffectDuration);
        enemy.ResetSpeedMultiplier();
        Debug.Log($"Slow effect ended for {enemy.name}.");
    }

    private void PlayAoEEffect()
    {
        if (aoeEffectPrefab != null && aoeEffectPosition != null)
        {
            GameObject effect = Instantiate(aoeEffectPrefab, aoeEffectPosition.position, Quaternion.identity);
            Destroy(effect, 6f); // Destroy the effect after a short time
        }
    }
}
