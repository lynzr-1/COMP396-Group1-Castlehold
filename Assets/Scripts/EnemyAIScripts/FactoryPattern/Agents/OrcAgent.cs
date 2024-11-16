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
        agent.destination = _destination;
    }
}
