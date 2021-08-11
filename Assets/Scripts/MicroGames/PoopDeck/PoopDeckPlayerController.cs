using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PoopDeckPlayerController : MicroGamePlayerController
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField, Tooltip("Degrees per second")]
    private float rotationSpeed = 90f;
    [SerializeField]
    private float mopSpeedMoveMultiplier = 1.5f;
    [SerializeField]
    private Transform mop;
    [SerializeField]
    private Animator mopAnimator;
    [SerializeField]
    private Animator mirroredPlayer;
    [SerializeField]
    private Animator mirroredMopAnimator;

    private Animator animator;

    private Vector3 movement;

    //! REMOVE
    [SerializeField]
    private int score;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        mirroredPlayer.applyRootMotion = false;

        // Initial mop
        Mop();
    }

    private void Update()
    {
        // Move the player...
        PlayerMovement();
    }

    private void FixedUpdate()
    {
        // Mop only on fixed update
        if (movement.magnitude > 0f)
        {
            Mop();
        }
    }

    private void LateUpdate()
    {
        mirroredPlayer.transform.localPosition = transform.localPosition;
        mirroredPlayer.transform.localRotation = transform.localRotation;
    }

    /// <summary>
    /// Handles the player input and movement
    /// </summary>
    private void PlayerMovement()
    {
        // Get the input
        movement = new Vector3(player.GetAxis("LeftMoveX"), 0f, player.GetAxis("LeftMoveY"));

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
        animator.SetFloat("Movement", movement.magnitude);
        mirroredPlayer.SetFloat("Movement", movement.magnitude);

        mopAnimator.SetFloat("Horizontal", -player.GetAxis("RightMoveX"));
        mopAnimator.SetFloat("Vertical", player.GetAxis("RightMoveY"));
        mirroredMopAnimator.SetFloat("Horizontal", -player.GetAxis("RightMoveX"));
        mirroredMopAnimator.SetFloat("Vertical", player.GetAxis("RightMoveY"));

        //mopAnimator.speed = 1f + Mathf.Clamp(movement.magnitude, 0f, 1f) * mopSpeedMoveMultiplier;
        //mirroredMopAnimator.speed = mopAnimator.speed;
    }

    private void Mop()
    {
        Ray ray = new Ray { origin = mop.position + transform.up, direction = -transform.up };
        score += PoopDeckManager.current.Mop(ray);
    }
}
