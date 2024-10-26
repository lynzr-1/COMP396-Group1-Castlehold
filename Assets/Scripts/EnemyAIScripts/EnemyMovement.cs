using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    //[SerializeField] private GameObject _startPoint;
    //[SerializeField] private GameObject _endPoint;
    [SerializeField] private Transform _destination;
 

    // Start is called before the first frame update
    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();

        if (_agent == null)
        {
            Debug.Log("The nav mesh agent is not assigned to " + gameObject.name);
        }
        else { SetDestination(); }
    }

    // Update is called once per frame
    private void SetDestination()
    {
        if (_destination != null)
        { 
            Vector3 targetVector = _destination.transform.position;
            _agent.SetDestination(targetVector);
        }
    }
}
