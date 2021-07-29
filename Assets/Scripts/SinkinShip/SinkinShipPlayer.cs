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
    public float gravMultiplier;
    private bool delay;
    public float delayTime;

    private Vector3 velocity;

    // upon start, setting variables for player
    private void Start()
    {
       
    }


    //  Update function every frame
    void Update()
    {

        var move = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Fire1") && delay == false)
        {
            velocity = -Physics.gravity.normalized * jumpVelocity;
            delay = true;
            StartCoroutine(DelayAction());
        }

        velocity += Physics.gravity * Time.deltaTime * gravMultiplier;
        controller.Move(velocity * Time.deltaTime);

        IEnumerator DelayAction()
        {
            yield return new WaitForSeconds(delayTime);
            delay = false;
        }
    }
}



