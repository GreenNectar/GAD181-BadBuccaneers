using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Chase : MonoBehaviour
{

    public Transform target;
    private NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        ChaseTarget();
    }

    void ChaseTarget()
    {
        // Set the agent to go to the currently selected target;
        agent.destination = target.position;
        // Set the agent speed to double when chasing target
        agent.speed = agent.speed * 2;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            ChaseTarget();


    }
}



