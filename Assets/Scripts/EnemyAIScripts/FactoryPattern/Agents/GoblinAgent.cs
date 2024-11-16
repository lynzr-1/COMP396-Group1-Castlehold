using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

//agent inherits from enemy behaviour and IAgent
public class GoblinAgent : EnemyBehaviour, IAgent
{
    [SerializeField] private NavMeshAgent _myself;
    [SerializeField] private Vector3 _destination;

    public void Navigate(Vector3 destination)
    {
        _destination = destination;
        agent.destination = _destination;
    }
}
