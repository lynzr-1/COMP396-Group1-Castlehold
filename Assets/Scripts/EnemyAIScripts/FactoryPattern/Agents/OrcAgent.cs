using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

//agent inherits from enemy behaviour and IAgent
public class OrcAgent : EnemyBehaviour, IAgent
{

    [SerializeField] private NavMeshAgent _myself;
    [SerializeField] private Vector3 _destination;

    public void Navigate(Vector3 destination) 
    {
        _destination = destination;
        StartCoroutine(SetDestinationWithDelay());
    }
    private IEnumerator SetDestinationWithDelay()
    {
        yield return null; // Wait a frame
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.destination = _destination;
        }
        else
        {
            Debug.LogError("NavMeshAgent is not active or not on NavMesh!");
        }
    }
}
