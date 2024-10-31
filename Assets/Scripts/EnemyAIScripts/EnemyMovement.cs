using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    //[SerializeField] private GameObject _startPoint;
    //[SerializeField] private GameObject _endPoint;
    [SerializeField] private Transform destination;
 

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.Log("The nav mesh agent is not assigned to " + gameObject.name);
        }
        else { SetDestination(); }
    }

    // Update is called once per frame
    private void SetDestination()
    {
        if (destination != null)
        { 
            Vector3 targetVector = destination.transform.position;
            agent.SetDestination(targetVector);
        }
    }
}
