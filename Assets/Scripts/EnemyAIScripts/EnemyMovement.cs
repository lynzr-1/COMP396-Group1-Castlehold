using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform endPoint;
    private Animator animator;
    private bool isAttacking;
    private Renderer enemyRenderer;

    private float reachThreshold = 1.0f; //min distance away from the end point the enemy must be to have "reached" it

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyRenderer = GetComponentInChildren<Renderer>();

        GameObject endPointObject = GameObject.FindGameObjectWithTag("EnemyPathEnd"); //get the end point/castle gate location by tag

        if (endPointObject != null)
        {
            endPoint = endPointObject.transform;  // Set the class-level endPoint to the end point transform
            agent.SetDestination(endPoint.position);  // Set the destination for the agent
        }
        else
        {
            Debug.LogError("End point not found. Please tag the end point with 'EnemyPathEnd'.");
        }
    }

    private void Update()
    {
        if (endPoint != null)
        {
            // Check if the enemy is within reach of the end point
            float distanceToEndPoint = Vector3.Distance(transform.position, endPoint.position);

            if (distanceToEndPoint <= reachThreshold)
            {
                ReachCastle();
            }
        }
    }

    private void ReachCastle()
    {
        Debug.Log("Enemy reached end point");

        //add logic here for damaging the castle and reducing it's health

        //stop the navmesh agent to halt movement at the gate
        agent.isStopped = true;

        //play attack animation
        animator.SetTrigger("Attack");

        //start the fade out coroutine after a delay to allow the attack animation time to play
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
        yield return new WaitForSeconds(delay);  // Wait for the attack animation to complete

        // Fade out over time
        float fadeDuration = 3.0f;  // Duration of the fade effect
        float elapsedTime = 0.0f;

        // Get the material color with alpha support
        Color originalColor = enemyRenderer.material.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration); // Calculate the alpha over time
            enemyRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha); // Apply alpha
            yield return null;
        }

        Destroy(gameObject);  // Destroy the enemy after fading out
    }
}
