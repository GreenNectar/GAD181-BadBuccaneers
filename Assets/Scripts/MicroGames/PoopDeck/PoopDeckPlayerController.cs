using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PoopDeckPlayerController : MicroGamePlayerController
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField, Tooltip("Degrees per second")]
    private float rotationSpeed = 90f;
    [SerializeField]
    private Transform mop;

    [SerializeField]
    private bool isDualStyle = false;

    //! REMOVE
    [SerializeField]
    private int score;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        // Move the player...
        PlayerMovement();

        // Mopping
        Ray ray = new Ray { origin = mop.position + transform.up, direction = -transform.up };

        score += PoopDeckTesting.current.Mop(ray);
    }

    /// <summary>
    /// Handles the player input and movement
    /// </summary>
    private void PlayerMovement()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isDualStyle = !isDualStyle;
        }

        //! I couldn't get the character controller to allow the player to stick to the boat ;-;
        //TODO Add collision detection

        

        if (!isDualStyle)
        {
            // Get the input
            Vector3 movement = new Vector3(player.GetAxis("LeftMoveX"), 0f, player.GetAxis("LeftMoveY"));

            // Movement, we only want to do it if we are pressing a button
            if (movement.magnitude > 0f)
            {
                // Get the rotations
                Quaternion wantedRotation = Quaternion.LookRotation(movement, Vector3.up); // Movement direction
                Quaternion currentRotation = transform.localRotation;
                Vector3 currentEuler = currentRotation.eulerAngles; // Get the euler so I don't have to write so much

                // Get the difference in angle
                float difference = Mathf.DeltaAngle(currentEuler.y, wantedRotation.eulerAngles.y);

                // We want the rotation delta to be based on how much we are moving
                float offset = rotationSpeed * Time.deltaTime * movement.magnitude;

                // Move towards the target angle, this is basically lerping between the current to the wanted
                if (Mathf.Abs(difference) > offset)
                {
                    if (difference > 0)
                    {
                        currentEuler.y += offset;
                    }
                    else
                    {
                        currentEuler.y -= offset;
                    }
                }
                else
                {
                    currentEuler.y = wantedRotation.eulerAngles.y;
                }

                // Set the rotation
                transform.localRotation = Quaternion.Euler(currentEuler);
            }

            // Move the player in the direction we are looking towards
            //characterController.Move(transform.forward * Time.deltaTime * speed * movement.magnitude);
            transform.Translate(transform.forward * Time.deltaTime * speed * movement.magnitude);
        }
        else
        {
            // Get the input
            Vector3 movement = new Vector3(player.GetAxis("LeftMoveX"), 0f, player.GetAxis("LeftMoveY"));
            Vector3 look = new Vector3(player.GetAxis("RightMoveX"), 0f, player.GetAxis("RightMoveY"));

            if (look.sqrMagnitude > 0f)
            {

                // Get the rotations
                Quaternion wantedRotation = Quaternion.LookRotation(look, Vector3.up); // Movement direction
                Quaternion currentRotation = transform.localRotation;
                Vector3 currentEuler = currentRotation.eulerAngles; // Get the euler so I don't have to write so much

                // Get the difference in angle
                float difference = Mathf.DeltaAngle(currentEuler.y, wantedRotation.eulerAngles.y + 180f);


                float offset = rotationSpeed * Time.deltaTime;

                // Move towards the target angle, this is basically lerping between the current to the wanted
                if (Mathf.Abs(difference) > offset)
                {
                    if (difference > 0)
                    {
                        currentEuler.y += offset;
                    }
                    else
                    {
                        currentEuler.y -= offset;
                    }
                }
                else
                {
                    currentEuler.y = wantedRotation.eulerAngles.y + 180f;
                }

                // Set the rotation
                transform.localRotation = Quaternion.Euler(currentEuler);
            }

            //characterController.Move(movement * Time.deltaTime * speed);
            transform.Translate(movement * Time.deltaTime * speed, transform.parent);
        }
    }
}
