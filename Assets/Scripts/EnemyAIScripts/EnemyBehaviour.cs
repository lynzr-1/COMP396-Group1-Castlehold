using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Attack Settings")]
    public float damage = 10f;
    public float speed = 5f;

    [Header("Enemy Health Settings")]
    public float maxHealth;

    private float _currentHealth;
    private bool _isDead = false;
    private float _currentSpeed;
    public PoolManager poolManager;

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

        //set the enemy's starting health
        _currentHealth = maxHealth;

        //set the enemy's starting speed
        _currentSpeed = speed;

        if (endPointObject != null)
        {
            endPoint = endPointObject.transform;
            agent.SetDestination(endPoint.position);
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
        if (_isDead) return;  //skip movement logic if the enemy is dead

        if (!hasReachedCastle && endPoint != null && Vector3.Distance(transform.position, endPoint.position) <= reachThreshold)
        {
            hasReachedCastle = true;
            ReachCastle();  // Call ReachCastle from EnemyBehaviour when reaching the end point
        }
    }

    public void ReachCastle()
    {

        agent.isStopped = true; // Stop the navmesh agent to halt movement at the gate
        animator.SetTrigger("Attack"); // Play attack animation
        castleHealthManager.TakeDamage(damage); //call the take damage function on the castle

        // Start the fade out coroutine after a delay to allow the attack animation time to play
        StartCoroutine(FadeOutAfterDelay(1.0f));
    }

    // Method to take damage - damage amount will be passed from towers
    public void TakeDamage(float damage)
    {
        if (_isDead) return;  // Prevent further damage if already dead

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void NotifySpawner()
    {
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.OnEnemyRemoved();
        }
    }

    private void OnDestroy()  // Call this when the enemy is destroyed or returned to the pool
    {
        NotifySpawner();
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        _currentSpeed = speed * multiplier;
        agent.speed = _currentSpeed;
        Debug.Log($"Enemy speed set to {_currentSpeed}");
    }

    public void ResetSpeedMultiplier()
    {
        _currentSpeed = speed;
        agent.speed = speed;
        Debug.Log($"Enemy speed reset to {speed}");
    }

    #region Attack/Die Anims & Coroutines to Fade After Delay

    //method to trigger death animation when enemy dies
    public void Die()
    {
        if ( _isDead ) return; //prevent multiple calls

        _isDead = true;

        //stop and disable the NavMeshAgent
        if (agent != null)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            agent.enabled = false;
        }

        //disable the collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        animator.SetTrigger("Die");  //trigger death animation

        // Notify the spawner that this enemy is removed
        NotifySpawner();

        //notify all towers about the enemy's destruction
        TowerAttack[] towers = FindObjectsOfType<TowerAttack>();

        foreach (TowerAttack tower in towers)
        {
            tower.NotifyEnemyDestroyed(gameObject);
        }

        //drop loot
        EnemyDrops drops = GetComponent<EnemyDrops>();
        if (drops != null)
        {
            PlayerGoldManager goldManager = FindObjectOfType<PlayerGoldManager>();
            if (goldManager != null)
            {
                drops.DropLoot();
            }
            else
            {
                Debug.LogError("PlayerGoldManager not found in the scene.");
            }
        }

        StartCoroutine(Destroy());  // Start coroutine to destroy object
    }

    //coroutine to destroy the enemy object either when it dies, or reaches the castle
    private IEnumerator Destroy()
    {
        // Wait until the "Die" animation is playing
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            yield return null;
        }

        // Wait for the length of the "Die" animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

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

    public void StartAttack(float duration)
    {
        if (isAttacking) return;  // Prevent multiple attacks at the same time
        if (_isDead) return;  // Do nothing if the enemy is dead

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
