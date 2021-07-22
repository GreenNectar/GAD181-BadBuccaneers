using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugglerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    // variables for player statistics
    public float moveSpeed;
  

    private Vector3 velocity;


    //  Update function every frame
    void Update()
    {
        var moveVertical = new Vector3(Input.GetAxis("Vertical"), 0f, 0f);
        controller.Move(moveVertical * moveSpeed * Time.deltaTime);

        //var moveHorizontal = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));
        //controller.Move(moveHorizontal * -moveSpeed * Time.deltaTime);


        velocity += Physics.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }
}
