using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class TopDownPlayerController : MicroGamePlayerController
{
    [SerializeField]
    private Camera camera;

    [SerializeField, HideIf("UsesRootMotion")]
    private float speed = 5f;

    [SerializeField]
    protected float animatorSpeed = 1.5f;

    [SerializeField]
    protected float lerpSpeed = 10f;

    [SerializeField]
    protected float gravity = 9.8f;

    protected float verticalSpeed = 0f;

    private Animator animator;
    protected Animator Animator
    {
        get
        {
            if (!animator) animator = GetComponent<Animator>();

            return animator;
        }
    }
    protected CharacterController characterController;

    private bool UsesRootMotion => Animator.applyRootMotion;

    protected Vector3 movement;

    protected CollisionFlags moveFlags;

    //private float currentSpeed;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        animator.speed = animatorSpeed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetInput();

        // Increase vertical speed by gravity
        verticalSpeed += gravity * Time.deltaTime;

        GroundCheck();

        PlayerLook();

        MovePlayer();

        SetAnimator();
    }

    protected void MovePlayer()
    {
        // If we are not using root motion, we use the speed to move the player
        if (!UsesRootMotion)
        {
            moveFlags = characterController.Move(((movement * speed) + (Vector3.down * verticalSpeed)) * Time.deltaTime);
        }
        else
        {
            moveFlags = characterController.Move(Vector3.down * verticalSpeed * Time.deltaTime);
        }
    }

    protected void PlayerLook()
    {
        if (movement.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
        }
    }

    protected virtual void SetAnimator()
    {
        // Set the animator movement
        animator.SetFloat("Movement", movement.magnitude);
    }

    protected void GetInput()
    {
        // Get the movement
        Vector3 newMovement = new Vector3(player.GetAxis("LeftMoveX"), 0f, player.GetAxis("LeftMoveY"));

        // Rotate the input by the cameras rotation
        float cos = Mathf.Cos(camera.transform.eulerAngles.y * Mathf.Deg2Rad);
        float sin = Mathf.Sin(camera.transform.eulerAngles.y * Mathf.Deg2Rad);
        Vector3 temp = new Vector3();
        temp.x = newMovement.x * cos - newMovement.z * sin;
        temp.z = newMovement.x * sin + newMovement.z * cos;

        // Set the new movement
        newMovement = temp;

        newMovement = newMovement.normalized * Mathf.Clamp(newMovement.magnitude, 0f, 1f);
        movement = lerpSpeed > 0f ? Vector3.Lerp(movement, newMovement, lerpSpeed * Time.deltaTime) : newMovement;
    }

    protected void GroundCheck()
    {
        // Apply gravity to the player
        if (characterController.isGrounded)
        {
            verticalSpeed = gravity * 0.5f;
        }
    }
}
