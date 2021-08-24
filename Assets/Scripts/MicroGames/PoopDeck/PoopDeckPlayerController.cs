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
        // Deactivate the mirrored player the player is not joined
        if (PlayerNumber > PlayerManager.PlayerCount - 1 && PlayerManager.PlayerCount != 0)
        {
            mirroredPlayer.gameObject.SetActive(false);
        }

        base.Start();

        animator = GetComponent<Animator>();
        mirroredPlayer.applyRootMotion = false;

        // Initial mop
        Mop();
    }

    private void OnDisable()
    {
        movement = Vector3.zero;
        SetAnimator();
    }

    private void Update()
    {
        // Get the input
        GetPlayerInput();

        // Move the player...
        PlayerMovement();

        // Set animator parameters
        SetAnimator();
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

    private void GetPlayerInput()
    {
        // Get the input
        movement = new Vector3(player.GetAxis("LeftMoveX"), 0f, player.GetAxis("LeftMoveY"));
    }

    private void SetAnimator()
    {
        animator.SetFloat("Movement", movement.magnitude);
        mirroredPlayer.SetFloat("Movement", movement.magnitude);
    }

    /// <summary>
    /// Handles the player input and movement
    /// </summary>
    private void PlayerMovement()
    {


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

        mirroredMopAnimator.SetFloat("Horizontal", -player.GetAxis("RightMoveX"));
        mirroredMopAnimator.SetFloat("Vertical", player.GetAxis("RightMoveY"));

        //mopAnimator.speed = 1f + Mathf.Clamp(movement.magnitude, 0f, 1f) * mopSpeedMoveMultiplier;
        //mirroredMopAnimator.speed = mopAnimator.speed;
    }

    private void Mop()
    {
        Ray ray = new Ray { origin = mop.position + transform.up, direction = -transform.up };
        int score = PoopDeckManager.current.Mop(ray);
        this.score += score;
        ScoreManager.Instance.AddScoreToPlayer(PlayerNumber, score);

        // Vibration
        Vibrator.Instance.PulseVibration(PlayerNumber, 1, 0.25f, Mathf.Clamp((float)score / 50f, 0f, 1f) * 0.2f);
    }
}
