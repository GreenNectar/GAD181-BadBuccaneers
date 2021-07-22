using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MicroGamePlayerController
{
    public float moveSpeed = 30f;       // the acceleration of the player
    public float jumpForce = 5f;        // the amount of force applied to make the player jump
    public float rotateSpeed = 10f;
    public float maxSpeed = 5f;
    public bool grounded = true;        // a boolean used to define whether the player is on the ground
    public GameObject avatar;

    float moveX;
    float moveZ;
    Rigidbody rb;
    bool stopZ;
    bool stopX;

    void FixedUpdate() // Update is called every frame
    {
        // MOVEMENT
        moveZ = player.GetAxis("LeftMoveY");
        moveX = player.GetAxis("LeftMoveX");
        rb = this.GetComponent<Rigidbody>(); //rb is the rigidbody attatched to this gameobject

        //APPLY FORCE

        if (moveZ != 0)
        {
            Vector3 moveVectorZ = transform.forward * moveZ;
            rb.AddForce(moveVectorZ * moveSpeed);
            Vector3 lookDirection = transform.position + rb.velocity;
            lookDirection.y = 0;
            avatar.gameObject.transform.LookAt(Vector3.Slerp(avatar.gameObject.transform.forward, lookDirection, 1f));
            stopZ = true;
        }

        else if(stopZ)
        {
            rb.AddForce(0f, 0f, -rb.velocity.z, ForceMode.VelocityChange); //STOP ON A DIME
            stopZ = false;
        }



        if (moveX != 0)
        {
            Vector3 moveVectorX = transform.right * moveX;
            rb.AddForce(moveVectorX * moveSpeed);
            Vector3 lookDirection = transform.position + rb.velocity;
            lookDirection.y = 0;
            avatar.gameObject.transform.LookAt(lookDirection);
            stopX = true;
        }

        else if (stopX)
        {
            rb.AddForce(-rb.velocity.x, 0f, 0f, ForceMode.VelocityChange); //STOP ON A DIME
            stopX = false;
        }

        // CAP VELOCITY AT MAX SPEED (but not on the y axis)

        float jumpSave = rb.velocity.y;                                        // jumpSave stores the current velocity of our y axis so that we can cap our speed without affecting jumping/falling
        Vector3 jumpVector = new Vector3(rb.velocity.x, 0, rb.velocity.z);     // jumpVector is a vector containing our current velocity with the y axis excluded

        if (rb.velocity.magnitude > maxSpeed)                                  // if the speed (magnitude) of our rigidbody is more than the value of our maximum speed...
        {
            rb.velocity = jumpVector.normalized * maxSpeed;                    // jumpvector is normalized so that direction is preserved but our magnitude (or speed rather) is now 1, it's then multipled by maxspeed to make the magnitude equal to the maximum speed allowed
            rb.velocity = new Vector3(rb.velocity.x, jumpSave, rb.velocity.z); // our stored y velocity is added back in so that jumping/falling is unaffected
        }
    }
}
