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
    private bool delay;
    public float delayTime;

    private Vector3 velocity;


    //  Update function every frame
    void Update()
    {
        var moveVertical = new Vector3(Input.GetAxis ("Vertical"), 0f, 0f);
        controller.Move(moveVertical * moveSpeed * Time.deltaTime);

        var moveHorizontal = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));
        controller.Move(moveHorizontal * -moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Fire1") && delay == false)
        {
            velocity = -Physics.gravity.normalized * jumpVelocity;
            delay = true;
            StartCoroutine(DelayAction());
        }

        velocity += Physics.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        IEnumerator DelayAction()
        {
            yield return new WaitForSeconds(delayTime);
            delay = false;
        }


    }
}



