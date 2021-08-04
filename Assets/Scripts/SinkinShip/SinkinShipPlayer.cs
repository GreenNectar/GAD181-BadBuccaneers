using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SinkinShipPlayer : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    // variables for player statistics
    public float moveSpeed;
    public float jumpVelocity;

    private Vector3 velocity;


    //  Update function every frame
    void Update()
    {

        var move = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            velocity = -Physics.gravity.normalized * jumpVelocity;
        }

        velocity += Physics.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }
}



