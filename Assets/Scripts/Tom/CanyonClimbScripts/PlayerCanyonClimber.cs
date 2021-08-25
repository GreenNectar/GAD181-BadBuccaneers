using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCanyonClimber : MicroGamePlayerController
{
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private Footsteps footsteps;

    Animator animator;

    // variables for player statistics
    public float moveSpeed = 5f;
    public float jumpVelocity = 5f;
    public float gravMultiplier = 1f;
    public float rotationSpeed = 1f;
    private Vector3 startingPosition;
    private Vector3 move;

    [SerializeField]
    private float coyoteTime = 0.25f;
    private float timeOffOfGround;
    private bool hasJumped;
    public bool isDrowned { get; private set; } = false;

    //private bool facingRight = true;
    // private bool isJumping = false;



    private Vector3 velocity;

    protected override void Start()
    {
        base.Start();
        startingPosition = transform.position;
        animator = GetComponent<Animator>();
        ScoreManager.Instance.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrowned) return;

        // Look towards camera if we are in the victory state
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Victory"))
        {
            float rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            return;
        }

        // Get the movement
        move = new Vector3(player.GetAxis("LeftMoveX"), 0f, 0f);
        // Move the player and get the collision info
        CollisionFlags flags = controller.Move(((move * moveSpeed) + velocity) * Time.deltaTime);
        // Add gravity after we move (goes weird if not)
        velocity += Physics.gravity * Time.deltaTime * gravMultiplier;

        // If we hit our head, go down
        if (flags == CollisionFlags.Above)
        {
            velocity = Physics.gravity * Time.deltaTime * gravMultiplier;
        }

        // Set the animator values
        animator.SetFloat("Movement", move.magnitude);
        animator.SetBool("Grounded", controller.isGrounded);
       
        // Only allow footsteps if on ground
        footsteps.enabled = controller.isGrounded;

        // Coyote time
        if (controller.isGrounded)
        {
            velocity = Physics.gravity * 0.5f;
            timeOffOfGround = 0f;
            hasJumped = false;
        }
        else
        {
            timeOffOfGround += Time.deltaTime;
        }

        // Jump
        if (hasJumped == false && timeOffOfGround <= coyoteTime)
        {
            if (player.GetButtonDown("Fire"))
            {
                hasJumped = true;
                velocity = -Physics.gravity.normalized * jumpVelocity;
                animator.SetTrigger("Jump");
                PlayerManager.GetPlayerFMODEvent(PlayerNumber).Jump(gameObject);
                Vibrator.Instance.ImpactVbration(PlayerNumber, 1, 0.25f, 0.5f);
            }
        }

        // Stop z drift
        controller.enabled = false;
        Vector3 temp = transform.position;
        transform.position = new Vector3(temp.x, temp.y, startingPosition.z);
        controller.enabled = true;

        SetCharacterRotation();
    }

    private void SetCharacterRotation()
    {
        Vector3 wantedMove = move;
        if (wantedMove.magnitude == 0f)
        {
            wantedMove = Vector3.back;
        }

        Quaternion wantedRotation = Quaternion.LookRotation(wantedMove, Vector3.up);
        Vector3 wantedEuler = wantedRotation.eulerAngles;
        wantedEuler.x = 0f;
        wantedEuler.z = 0f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(wantedEuler), Time.deltaTime * rotationSpeed);

    }

    public void Drown()
    {
        isDrowned = true;
        animator.SetTrigger("Drown");

        Vibrator.Instance.ImpactVbration(PlayerNumber, 0, 1f, 1f);

        PlayerManager.GetPlayerFMODEvent(PlayerNumber).Drowning(transform.position);

        if (FindObjectsOfType<PlayerCanyonClimber>().Where(p => p.isDrowned).Count() == 4)
        {
            GameManager.EndGameStatic(2.5f);
            timer.StopTimer();
        }
    }
}


