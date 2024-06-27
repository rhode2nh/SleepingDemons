using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    void Start()
    {
        agent.SetDestination(Random.insideUnitCircle * 3);
    }

    void Update()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial || !agent.hasPath)
        {
            agent.SetDestination(Random.insideUnitCircle * 10);
        }
    }
}
