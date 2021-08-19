using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class TopDownPlayerController : MicroGamePlayerController
{
    [SerializeField, HideIf("UsesRootMotion")]
    private float speed = 5f;

    [SerializeField]
    protected float animatorSpeed = 1.5f;

    [SerializeField]
    protected float lerpSpeed = 10f;

    [SerializeField]
    private float gravity = 9.8f;

    private Animator animator;
    protected Animator Animator
    {
        get
        {
            if (!animator) animator = GetComponent<Animator>();

            return animator;
        }
    }
    private CharacterController characterController;

    private bool UsesRootMotion => Animator.applyRootMotion;

    private Vector3 movement;

    [Header("Audio")]
    [SerializeField, EventRef]
    private string walkEvent;
    private float walkDistance;
    private float maxWalk = 0.8f;

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
        // Get the movement
        Vector3 newMovement = new Vector3(player.GetAxis("LeftMoveX"), 0f, player.GetAxis("LeftMoveY"));
        newMovement = newMovement.normalized * Mathf.Clamp(newMovement.magnitude, 0f, 1f);
        movement = lerpSpeed > 0f ? Vector3.Lerp(movement, newMovement, lerpSpeed * Time.deltaTime) : newMovement;

        // Set the animator movement
        animator.SetFloat("Movement", movement.magnitude);

        if (movement.magnitude > 0.01f)
        {
            //Quaternion wantedRotation = Quaternion.LookRotation(movement, Vector3.up);
            // Rotate towards where we are walking towards
            //transform.rotation = lerpSpeed > 0f ? Quaternion.Lerp(transform.rotation, wantedRotation, lerpSpeed) : wantedRotation;
            transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
        }

        // Apply gravity to the player
        characterController.Move(Vector3.down * gravity * Time.deltaTime);

        Vector3 moveVector = movement * speed * Time.deltaTime;

        // If we are not using root motion, we use the speed to move the player
        if (!UsesRootMotion)
        {
            characterController.Move(moveVector);
        }

        // Play the walk sound
        walkDistance += moveVector.magnitude;
        if (walkDistance > maxWalk)
        {
            walkDistance %= maxWalk;;
            RuntimeManager.PlayOneShotAttached(walkEvent, gameObject);
        }
    }
}
