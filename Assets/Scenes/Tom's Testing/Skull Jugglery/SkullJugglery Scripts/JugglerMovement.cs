using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugglerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    public float farRight;
    public float farLeft;

    // variables for player statistics
    public float moveSpeed;

    //private Vector3 velocity;



    //  Update function every frame
    void Update()
    {
        var moveHorizontal = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));
        controller.Move(moveHorizontal * -moveSpeed * Time.deltaTime);

        // This clamps the flag on the z axis
        Vector3 clampedPosition = transform.position;
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, farLeft, farRight);
        transform.position = clampedPosition;

    }
}
