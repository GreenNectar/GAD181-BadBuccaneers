using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CaveRaseMovement : MicroGamePlayerController
{
    CharacterController characterController;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Animator anim;
    public bool dead = false;
    public float fowardSpeed = 1f;

    private Vector3 moveDirection = Vector3.zero;

    private float startingHeight;
    private float previousHeight;
    [SerializeField]
    bool delay;

    protected override void Start()
    {
        base.Start();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        startingHeight = characterController.height;
        delay = false;
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(moveDirection * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;
        if(dead == false)
        {
            characterController.Move(Vector3.forward * Time.deltaTime * fowardSpeed);
        }

        if (player.GetButtonDown("Jump") && delay == false && dead == false)
        {
            if (characterController.velocity.y == 0)
            {
                moveDirection.y = jumpSpeed;
                anim.SetTrigger("Jumped");
                Debug.Log("Jumping");
                delay = true;
                StartCoroutine(DelayAction());
            }
           // moveDirection.y = jumpSpeed;
            Debug.Log("Pressed Jump");
        }

        if(player.GetButtonDown("Duck") && delay == false && dead == false)
        {
            anim.SetTrigger("Ducked");
            delay = true;
            StartCoroutine(DelayAction());
        }

        // Change the capsule's height to slide
        float characterRadius = characterController.radius;
        float newHeight = anim.GetBool("IsDucked") ? characterRadius : startingHeight;
        characterController.height = newHeight;

        // Keep the bottom of the collider on the ground
        if (newHeight != previousHeight)
        {
            characterController.Move(Vector3.down * characterController.radius * (newHeight == characterRadius ? 1f : -1f));
            previousHeight = newHeight;
        }

        // You might want to have a custom camera follow script that follows the x and z but not y
        // The reason the camera flicks up and down is because the origin moves up and down
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trip"))
        {
            anim.SetTrigger("Tripped");
            dead = true;
        }
        
        if (other.CompareTag("HitAbove"))
        {
            anim.SetTrigger("Died");
            dead = true;
        }
    }

    IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(0.9f);
        delay = false;
    }
}
