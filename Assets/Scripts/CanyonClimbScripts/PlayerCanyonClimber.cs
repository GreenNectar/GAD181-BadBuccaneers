using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanyonClimber : MicroGamePlayerController
{
    [SerializeField]
    private CharacterController controller;

    // variables for player statistics
    public float moveSpeed = 5f;
    public float jumpVelocity = 5f;
    public float gravMultiplier = 1f;
    public float rotationSpeed = 1f;
    private Vector3 startingPosition;
    private Vector3 move;

    //private bool facingRight = true;
    // private bool isJumping = false;



    private Vector3 velocity;
    private Vector3 startingPosition;

    protected override void Start()
    {
        base.Start();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var move = new Vector3(player.GetAxis("LeftMoveX"), 0f, 0f);
        move = new Vector3(player.GetAxis("LeftMoveX"), 0f, 0f);
        controller.Move(move * moveSpeed * Time.deltaTime);
        velocity += Physics.gravity * Time.deltaTime * gravMultiplier;
        CollisionFlags collisionFlags = controller.Move(velocity * Time.deltaTime);
        CollisionFlags flags = controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            if (player.GetButtonDown("Fire"))
            {
                velocity = -Physics.gravity.normalized * jumpVelocity;
            }
        }
        else if (collisionFlags == CollisionFlags.Above)

        if (flags == CollisionFlags.Above)
        {
            velocity = Physics.gravity * Time.deltaTime;
            velocity = Physics.gravity * Time.deltaTime * gravMultiplier;
        }

        /* 
         if (move > 0 && !facingRight)
         {
             FlipCharacter();
         }
        controller.enabled = false;
        Vector3 temp = transform.position;
        transform.position = new Vector3(temp.x, temp.y, startingPosition.z);
        controller.enabled = true;

         else if (move < 0 && facingRight)
         {
             FlipCharacter();
         }
        */

        /* if (Input.GetButtonDown("Jump"))
         {
             isJumping = true;
         }
        */
        SetCharacterRotation();
    }

        //isGrounded = Physics.OverlapSphere(groundCheck.position, checkRadius, groundObjects);
    private void SetCharacterRotation()
    {
        Vector3 wantedMove = move;
        if (wantedMove.magnitude == 0f)
        {
            wantedMove = Vector3.back;
        }

        // This clamps the flag on the y axis
        controller.enabled = false;
        Vector3 temp = transform.position;
        transform.position = new Vector3(temp.x, temp.y, startingPosition.z);
        controller.enabled = true;
    }
        Quaternion wantedRotation = Quaternion.LookRotation(wantedMove, Vector3.up);
        Vector3 wantedEuler = wantedRotation.eulerAngles;
        wantedEuler.x = 0f;
        wantedEuler.z = 0f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(wantedEuler), Time.deltaTime * rotationSpeed);


    /*
     private void FlipCharacter()
     {
         facingRight = !facingRight;
         transform.Rotate(0f, 180f, 0f);
     }
    */

    

    }
}
