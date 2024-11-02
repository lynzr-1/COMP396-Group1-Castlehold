using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator animator;
    private bool isAttacking;
    public PoolManager poolManager;  // Reference to the pool manager

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    public void ReachCastle()
    {
        Debug.Log("Enemy reached end point");

        // Stop the navmesh agent to halt movement at the gate
        agent.isStopped = true;

        // Play attack animation
        animator.SetTrigger("Attack");

        // Start the fade out coroutine after a delay to allow the attack animation time to play
        StartCoroutine(FadeOutAfterDelay(1.0f));
    }

    public void StartAttack(float duration)
    {
        if (isAttacking) return;  // Prevent multiple attacks at the same time

        isAttacking = true;
        animator.SetBool("IsAttacking", true);  // Set to loop the Attack state
        StartCoroutine(StopAttackAfterTime(duration));
    }

    private IEnumerator StopAttackAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);  // Wait for the attack duration

        animator.SetBool("IsAttacking", false);  // Stop looping
        isAttacking = false;
    }

    private IEnumerator FadeOutAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Implement fade-out logic
        Renderer renderer = GetComponent<Renderer>();
        float fadeDuration = 2.0f;
        Color initialColor = renderer.material.color;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            Color newColor = initialColor;
            newColor.a = Mathf.Lerp(1, 0, t / fadeDuration);
            renderer.material.color = newColor;
            yield return null;
        }
        // Return the object to the pool
        if (poolManager != null)
        {
            poolManager.ReturnObject(gameObject);
        }
        else
        {
            Debug.LogWarning("Pool manager not assigned. Destroying object.");
            Destroy(gameObject);
        }
    }
}
