using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingTopDownPlayerController : TopDownPlayerController
{
    [SerializeField]
    private float jumpSpeed;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        GetInput();
        MovePlayer();
        PlayerLook();

        if (moveFlags == CollisionFlags.Above)
        {
            verticalSpeed = gravity * Time.deltaTime;
        }

        // Increase vertical speed by gravity
        verticalSpeed += gravity * Time.deltaTime;

        GroundCheck();

        if (characterController.isGrounded && player.GetButtonDown("Jump"))
        {
            verticalSpeed = -jumpSpeed;
            Animator.SetTrigger("Jump");
        }

        SetAnimator();
    }

    protected override void SetAnimator()
    {
        base.SetAnimator();
        Animator.SetBool("Grounded", characterController.isGrounded);
    }
}
