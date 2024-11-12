using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float damage = 10f;
    public float speed = 5f;

    public PoolManager poolManager;  // Reference to the pool manager

    #region Private Variables
    protected NavMeshAgent agent;
    protected Animator animator;
    protected CastleHealthManager castleHealthManager;
    private bool isAttacking;
    private Renderer enemyRenderer;
    private Transform endPoint;
    private float reachThreshold = 1.0f;
    private bool hasReachedCastle = false; //to ensure reach castle is only called once when the enemy reaches it
    #endregion

    private void Start()
    {
        // Set the end point for the enemy to reach
        GameObject endPointObject = GameObject.FindGameObjectWithTag("EnemyPathEnd");

        if (endPointObject != null)
        {
            endPoint = endPointObject.transform;
            agent.SetDestination(endPoint.position);  // Use inherited agent from EnemyBehaviour
        }
        else
        {
            Debug.LogError("End point not found. Please tag the end point with 'EnemyPathEnd'.");
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        castleHealthManager = FindObjectOfType<CastleHealthManager>();
    }

    private void Update()
    {
        if (!hasReachedCastle && endPoint != null && Vector3.Distance(transform.position, endPoint.position) <= reachThreshold)
        {
            hasReachedCastle = true;
            Debug.Log("Reached Castle");
            ReachCastle();  // Call ReachCastle from EnemyBehaviour when reaching the end point
        }
    }

    public void ReachCastle()
    {

        // Stop the navmesh agent to halt movement at the gate
        agent.isStopped = true;

        // Play attack animation
        animator.SetTrigger("Attack");
        Debug.Log("Calling Take Damage on castle");
        castleHealthManager.TakeDamage(damage); //call the take damage function on the castle

        // Start the fade out coroutine after a delay to allow the attack animation time to play
        StartCoroutine(FadeOutAfterDelay(1.0f));
    }

    #region Attack Anims & Coroutines to Fade After Delay
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
    #endregion
}
