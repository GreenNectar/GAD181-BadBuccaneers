using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;

public class HeistTheBootyPlayerController : MicroGamePlayerController
{

    public Animator animator;

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    [SerializeField]
    private HeistTheBootyController bootyController;
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>(); 
    }


    void Update()
    {
        if (player.GetButtonDown("ChickenOut"))
        {
            if (bootyController.ChickenOut(PlayerNumber))
            {
                animator.SetTrigger("Flee");
                agent.destination = points[destPoint].position;
                Debug.Log("Chickened");
            }
        }

    }
}
