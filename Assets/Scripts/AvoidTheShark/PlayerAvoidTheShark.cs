using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAvoidTheShark : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    // variables for player statistics
    public float moveSpeed;
    public float jumpVelocity;

    private Vector3 velocity;

    // upon start, setting variables for player
    private void Start()
    {

    }


    //  Update function every frame
    void Update()
    {
        var moveVertical = new Vector3(0f, 0f, Input.GetAxis ("Vertical"));
        controller.Move(moveVertical * moveSpeed * Time.deltaTime);

        var moveHorizontal = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        controller.Move(moveHorizontal * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            velocity = -Physics.gravity.normalized * jumpVelocity;
        }

        velocity += Physics.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }
}



