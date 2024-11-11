using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

//agent inherits from enemy behaviour and IAgent
public class SkeletonAgent : EnemyBehaviour, IAgent
{

    [SerializeField] private NavMeshAgent _myself;
    [SerializeField] private Vector3 _destination;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _destination) < 1.5f)
        {
            CompleteTask();
        }
    }

    public void Navigate(Vector3 destination)
    {
        _destination = destination;
        agent.destination = _destination;
    }

    public void CompleteTask()
    {
        Debug.Log("Skeleton has reached the castle");
        ReachCastle();  // Call ReachCastle from EnemyBehaviour
    }
}
