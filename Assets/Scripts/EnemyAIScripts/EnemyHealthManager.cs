using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;
    private Animator animator;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Method to take damage - damage amount will be passed from towers
    public void TakeDamage(int damage)
    {
        if (isDead) return;  // Prevent further damage if already dead

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    //method to trigger death animation when enemy dies
    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");  // Trigger death animation
        StartCoroutine(Destroy());  // Start coroutine to destroy object
    }

    //coroutine to destroy the enemy object either when it dies, or reaches the castle
    private IEnumerator Destroy()
    {
        // Wait for the length of the die animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);  // Destroy the enemy object
    }
}
