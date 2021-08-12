using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JugglerMovement : MicroGamePlayerController

{
    [SerializeField]
    private CharacterController controller;
    public float farRight;
    public float farLeft;

    // variables for player statistics
    public float moveSpeed = 7;
    public float jumpVelocity = 5f;
    public float gravMultiplier = 1f;

    public float rotationSpeed = 1f;
    private Vector3 startingPosition;
    private Vector3 move;
    private Vector3 velocity;


    protected override void Start()
    {
        base.Start();
        startingPosition = transform.position;
    }


    void Update()
    {
        move = new Vector3(player.GetAxis("LeftMoveX"), 0f, 0f);
        controller.Move(move * moveSpeed * Time.deltaTime);
        velocity += Physics.gravity * Time.deltaTime * gravMultiplier;
        CollisionFlags flags = controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            if (player.GetButtonDown("Fire"))
            {
                velocity = -Physics.gravity.normalized * jumpVelocity;
            }
        }

        if (flags == CollisionFlags.Above)
        {
            velocity = Physics.gravity * Time.deltaTime * gravMultiplier;
        }

        controller.enabled = false;
        Vector3 temp = transform.position;
        transform.position = new Vector3(temp.x, temp.y, startingPosition.z);
        controller.enabled = true;

        SetCharacterRotation();

        // This clamps the flag on the x axis
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, farLeft, farRight);
        transform.position = clampedPosition;
    }

    private void SetCharacterRotation()
    {
        Vector3 wantedMove = move;
        if (wantedMove.magnitude == 0f)
        {
            wantedMove = Vector3.back;
        }

        Quaternion wantedRotation = Quaternion.LookRotation(wantedMove, Vector3.up);
        Vector3 wantedEuler = wantedRotation.eulerAngles;
        wantedEuler.x = 0f;
        wantedEuler.z = 0f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(wantedEuler), Time.deltaTime * rotationSpeed);

    }



    //  Update function every frame
    /*void Update()
    {
        var moveHorizontal = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));
        controller.Move(moveHorizontal * -moveSpeed * Time.deltaTime);

        // This clamps the flag on the z axis
        Vector3 clampedPosition = transform.position;
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, farLeft, farRight);
        transform.position = clampedPosition;

    }
    */
}
