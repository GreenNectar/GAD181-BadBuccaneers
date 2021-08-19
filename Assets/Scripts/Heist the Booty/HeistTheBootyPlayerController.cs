using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeistTheBootyPlayerController : MicroGamePlayerController
{
    [SerializeField]
    private HeistTheBootyController bootyController;
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (player.GetButtonDown("ChickenOut"))
        {
            bootyController.ChickenOut(PlayerNumber);
        }
     
    }
}
