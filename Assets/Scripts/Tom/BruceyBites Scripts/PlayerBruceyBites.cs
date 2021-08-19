using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBruceyBites : JumpingTopDownPlayerController
{
    protected override void Update()
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Victory"))
        {
            float rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            return;
        }

        base.Update();
    }

    //[SerializeField]
    //private CharacterController controller;

    //Animator animator;

    //// variables for player statistics
    //public float moveSpeed = 5f;
    //public float jumpVelocity = 5f;
    //public float gravMultiplier = 2f;
    //public float rotationSpeed = 1f;
    //private Vector3 startingPosition;
    //private Vector3 move;

    //private Vector3 velocity;

    //protected override void Start()
    //{
    //    base.Start();
    //    startingPosition = transform.position;
    //    animator = GetComponent<Animator>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Victory"))
    //    {
    //        float rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up).eulerAngles.y;
    //        transform.rotation = Quaternion.Euler(0f, rotation, 0f);
    //        return;
    //    }

    //    move = new Vector3(player.GetAxis("LeftMoveX"), 0f, player.GetAxis("LeftMoveY"));
    //    CollisionFlags flags = controller.Move(((move * moveSpeed) + velocity) * Time.deltaTime);
    //    velocity += Physics.gravity * Time.deltaTime * gravMultiplier;

    //    if (flags == CollisionFlags.Above)
    //    {
    //        velocity = Physics.gravity * Time.deltaTime * gravMultiplier;
    //    }

    //    if (controller.isGrounded)
    //    {
    //        velocity = Physics.gravity * 0.5f;
    //    }


    //    animator.SetFloat("Movement", move.magnitude);
    //    animator.SetBool("Grounded", controller.isGrounded);

    //    if (controller.isGrounded)
    //    {
    //        if (player.GetButtonDown("Fire"))
    //        {
    //            velocity = -Physics.gravity.normalized * jumpVelocity;
    //            animator.SetTrigger("Jump");
    //        }
    //    }

    //    SetCharacterRotation();
    //}

    //private void SetCharacterRotation()
    //{
    //    Vector3 wantedMove = move;
    //    if (wantedMove.magnitude == 0f)
    //    {
    //        wantedMove = Vector3.back;
    //    }

    //    Quaternion wantedRotation = Quaternion.LookRotation(wantedMove, Vector3.up);
    //    Vector3 wantedEuler = wantedRotation.eulerAngles;
    //    wantedEuler.x = 0f;
    //    wantedEuler.z = 0f;
    //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(wantedEuler), Time.deltaTime * rotationSpeed);

    //}
}
