using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMovement : MicroGamePlayerController
{
    [SerializeField]
    public CharacterController controller;

    Animator animator;

    public float mashDelay = .5f;
    public float flagSpeed = 1f;
    public float dropSpeed = 0.5f;
    public float verticalSpeed;
    public float topHeight;
    public float mash;


    bool pressed;
    bool started;
    public bool canMove;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        mash = mashDelay;
        canMove = true;
    }


    // Update is called once per frame
    // flag moves up with each fire button
    public void Update()
    {
        // Wait until waitTime is below or equal to zero.
        if (mash > 0)
        {
            mash -= Time.deltaTime;
        }

        if (player.GetButtonDown("Fire"))
        {
            started = true;
        }

        if (started)
        {
            mash -= Time.deltaTime;

            // if the space bar is not pressed within the delay time, the flag moves down
            if (player.GetButtonDown("Fire") && !pressed && canMove)
            {
                pressed = true;
                mash = mashDelay;
                // transform.position += verticalSpeed * flagSpeed;
                verticalSpeed = flagSpeed;
            }
            else if (player.GetButtonDown("Fire"))
            {
                pressed = false;
            }

            //Flag moves down
            if (mash <= 0)
            {
                // transform.position -= transform.up * dropSpeed;
                verticalSpeed -= dropSpeed * Time.deltaTime;
            }

            transform.position += transform.up * verticalSpeed * Time.deltaTime;

            //This clamps the flag on the y axis
            Vector3 clampedPosition = transform.position;
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, 3f, topHeight);
            transform.position = clampedPosition;

        }

    }
}
