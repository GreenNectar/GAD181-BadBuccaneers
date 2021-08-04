using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanyonClimber : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    // variables for player statistics
    public float moveSpeed;
    public float jumpVelocity;
    public float gravMultiplier;
    private bool isGrounded;

    //private bool facingRight = true;
    // private bool isJumping = false;



    private Vector3 velocity;


    // Update is called once per frame
    void Update()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity = -Physics.gravity.normalized * jumpVelocity;
        }

        velocity += Physics.gravity * Time.deltaTime * gravMultiplier;
        controller.Move(velocity * Time.deltaTime);


       /* 
        if (move > 0 && !facingRight)
        {
            FlipCharacter();
        }

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

        //isGrounded = Physics.OverlapSphere(groundCheck.position, checkRadius, groundObjects);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }


    /*
     private void FlipCharacter()
     {
         facingRight = !facingRight;
         transform.Rotate(0f, 180f, 0f);
     }
    */
}
