using System.Collections;
using UnityEngine;

public class EnemyMovement : EnemyBehaviour  // Inherits from EnemyBehaviour
{
    private Transform endPoint;
    private float reachThreshold = 1.0f;

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

    private void Update()
    {
        if (endPoint != null)
        {
            // Check if the enemy is within reach of the end point
            float distanceToEndPoint = Vector3.Distance(transform.position, endPoint.position);

            if (distanceToEndPoint <= reachThreshold)
            {
                ReachCastle();  // Call ReachCastle from EnemyBehaviour when reaching the end point
            }
        }
    }
}
