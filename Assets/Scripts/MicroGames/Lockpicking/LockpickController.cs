using FMODUnity;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEngine.InputSystem;
//using static UnityEngine.InputSystem.InputAction;
using Random = UnityEngine.Random;

public class LockpickController : MicroGamePlayerController, IMicroGameLoad
{
    #region General

    [Header("General")]
    [SerializeField]
    private float speed = 1f;
    [Header("Pick Transforms")]
    [SerializeField, Tooltip("How far away the picks are from the correct positions that still count as a success")]
    private float correctDistance = 15f;
    [SerializeField, Tooltip("")]
    private float vibrationDistance = 30f;

    [Header("Pick Transforms")]
    [SerializeField]
    private Transform pickLeft;
    [SerializeField]
    private Transform pickRight;
    [SerializeField]
    private Transform pickPosition;

    [SerializeField, Tooltip("The correct rotation of the left pick")]
    private float correctRotationLeft = 0f;
    [SerializeField, Tooltip("The correct rotation of the right pick")]
    private float correctRotationRight = 180f;

    // The rotation that we want to be at
    [SerializeField]
    private float rotationLeft = 0f;
    [SerializeField]
    private float rotationRight = 0f;

    // The rotation we are currently at
    [SerializeField]
    private float currentRotationLeft = 0f;
    [SerializeField]
    private float currentRotationRight = 0f;

    // The current lock we are picking
    private int currentLock = 0;
    private int currentChest = 0;
    private int currentPin = 0;

    #endregion

    #region Components

    //private PlayerInput playerInput;
    private Animator animator;

    #endregion

    #region Properties

    private bool canOpen = false;
    public void AllowOpening()
    {
        canOpen = true;
    }
    public void DisallowOpening()
    {
        canOpen = false;
    }

    private float DistanceFromCorrect
    {
        get
        {
            float leftDiff = Mathf.Abs(Mathf.DeltaAngle(currentRotationLeft, correctRotationLeft)); // Get the angle distance of the left pick
            float rightDiff = Mathf.Abs(Mathf.DeltaAngle(currentRotationRight, correctRotationRight)); // Get the angle distance of the right pick
            float diff = leftDiff + rightDiff; // Add the differences
            return diff;
        }
    }

    #endregion

    #region Pins

    [Serializable]
    private class Pin
    {
        public Transform[] pins;
        public float heightOffsetWhenComplete = 0.1f;
        public bool isOpened = false;
    }

    [Serializable]
    private class Chest
    {
        public Pin[] pins;
    }

    [SerializeField]
    private Chest[] chests;

    private int NumberOfPins
    {
        get
        {
            int pins = 0;
            foreach (var chest in chests)
            {
                pins += chest.pins.Length;
            }
            return pins;
        }
    }

    #endregion

    #region Static Values

    // Made it static so all other instances can use it (keeps the randomness the same on all instances)
    private static float[] randomValues;

    #endregion

    #region Animator Methods

    public void StartTimer()
    {
        ScoreManager.Instance.StartTimer();
    }

    public void Finish()
    {
        ScoreManager.Instance.EndPlayer(PlayerNumber);
    }

    #endregion

    #region Audio

    [Header("Audio")]
    [SerializeField, EventRef]
    private string openChestEvent;
    [SerializeField, EventRef]
    private string openLastChestEvent;
    [SerializeField, EventRef]
    private string unlockPinEvent;

    #endregion

    public void OnMicroGameLoad()
    {
        // Generate the random values babbyy, that way they're different each time
        GenerateRandomRotations();
    }

    private void OnDisable()
    {
        DisableVibrations();
    }

    private void OnDestroy()
    {
        DisableVibrations();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        currentRotationLeft = rotationLeft;
        currentRotationRight = rotationRight;

        if (randomValues == null)
        {
            GenerateRandomRotations();
        }

        SetCorrectLockRotations();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        if (!canOpen)
        {
            player.SetVibration(0, 0f);
            player.SetVibration(1, 0f);
            return;
        }
        else
        {
            float leftVibration = 0f; // Big motor
            float rightVibration = 0f; // Small motor

            // This is the vibration that has to have both of the picks in the correct rotation (with 'vibrationDistance' maximum degrees between them)
            // The vibration is scaled to how close it is to the correct positions
            if (DistanceFromCorrect <= vibrationDistance) // If the difference is lower than the vibration distance, we want to set the vibration
            {
                // The vibration amount is the inverse distance from the max distance over the distance. This will turn (0 to vibrationDistance degrees to 0 to 1)
                float vibrationAmount = (vibrationDistance - DistanceFromCorrect) / vibrationDistance;
                vibrationAmount *= vibrationAmount; // Square it, this makes the transitions feel a bit nicer a(also graph like this _/\_)
                // We want to have the vibration spike when we are within the correct position buffer
                if (DistanceFromCorrect <= correctDistance)
                {
                    //vibrationAmount = 1f;
                    leftVibration = 1f;
                    rightVibration = 1f;
                }
                else
                {
                    rightVibration = vibrationAmount;
                }

                // Set it brother
                player.SetVibration(0, leftVibration);
                player.SetVibration(1, rightVibration);
            }
            else
            {
                player.SetVibration(0, 0f);
                player.SetVibration(1, 0f);
            }

            // Lerp the rotation to the inputted
            currentRotationRight += Mathf.Clamp(Mathf.DeltaAngle(currentRotationRight, rotationRight), -Time.deltaTime * speed, Time.deltaTime * speed);
            currentRotationLeft += Mathf.Clamp(Mathf.DeltaAngle(currentRotationLeft, rotationLeft), -Time.deltaTime * speed, Time.deltaTime * speed);

            // Set the rotation
            pickRight.rotation = Quaternion.Euler(0, 0, currentRotationRight);
            pickLeft.rotation = Quaternion.Euler(0, 0, currentRotationLeft);
        }

        //? Do I keep this and maybe have an option in the options manager (gotta make one), or just remove it? :\
        // This is the vibration that the left and right picks vibrate the left and right vibrations (respectively) regardless of the others position
        //float leftVibration = 0;
        //float rightVibration = 0;
        //if (Mathf.Abs(Mathf.DeltaAngle(currentRotationLeft, correctRotationLeft)) <= vibrationDistance)
        //{
        //    leftVibration = Mathf.Clamp(vibrationDistance - Mathf.Abs(Mathf.DeltaAngle(currentRotationLeft, correctRotationLeft)), 0, vibrationDistance) / vibrationDistance;
        //}
        //if (Mathf.Abs(Mathf.DeltaAngle(currentRotationRight, correctRotationRight)) <= vibrationDistance)
        //{
        //    rightVibration = Mathf.Clamp(vibrationDistance - Mathf.Abs(Mathf.DeltaAngle(currentRotationRight, correctRotationRight)), 0, vibrationDistance) / vibrationDistance;
        //}
        //if (Mathf.DeltaAngle(currentRotationLeft, correctRotationLeft) < 10f)
        //{

        //}
        //gamePad.SetMotorSpeeds(leftVibration * maxVibration, rightVibration * maxVibration);
    }

    private void GetInput()
    {
        Vector2 leftAxis = new Vector2(player.GetAxisRaw("LeftMoveX"), player.GetAxisRaw("LeftMoveY"));
        Vector2 rightAxis = new Vector2(player.GetAxisRaw("RightMoveX"), player.GetAxisRaw("RightMoveY"));

        if (leftAxis.sqrMagnitude > 0.25f)
            rotationLeft = leftAxis.Angle();
        if (rightAxis.sqrMagnitude > 0.25f)
            rotationRight = rightAxis.Angle();
        if (player.GetButtonDown("Fire"))
        {
            CheckLock();
        }
    }

    private void GenerateRandomRotations()
    {
        // Create the random rotations
        Random.InitState(DateTime.Now.GetHashCode());
        randomValues = new float[NumberOfPins * 2]; //[6]; // We have six values as we need two per lock (left and right)
        for (int i = 0; i < randomValues.Length; i++)
        {
            randomValues[i] = Random.Range(0f, 1080f) % 360f; // I wasn't sure if randomness weirdness was gonna prioritise the middle so I did a thing
        }
    }

    private void CheckLock()
    {
        // Check if we are allowed to open
        if (!canOpen) return;

        if (DistanceFromCorrect <= correctDistance)
        {
            // Move pin by the offset for that pin-set
            if (currentPin < chests[currentChest].pins.Length)
            {
                Pin pin = chests[currentChest].pins[currentPin];
                foreach (var p in pin.pins)
                {
                    p.transform.position += Vector3.up * pin.heightOffsetWhenComplete;
                }
            }

            // Increment pin
            currentPin++;

            // If we have finished all pins then we open the chest
            if (currentPin >= chests[currentChest].pins.Length)
            {
                currentPin = 0;
                currentChest++;
                animator.SetTrigger("Open");
                if (currentChest == chests.Length) // If we open last chest
                    RuntimeManager.PlayOneShot(openLastChestEvent);
                else // If we open normal chest
                    RuntimeManager.PlayOneShot(openChestEvent);
            }
            // If we didn't open the chest, play the unlock sound
            else
            {
                RuntimeManager.PlayOneShot(unlockPinEvent);
            }

            if (currentLock < NumberOfPins - 1) currentLock++; // We only have three locks atm, this is hard-coded... should not be that way (I have - 1 so it's obvious there is three locks)
            SetCorrectLockRotations();
        }
    }

    private void DisableVibrations()
    {
        if (ReInput.isReady)
        {
            player.SetVibration(0, 0f);
            player.SetVibration(1, 0f);
        }
    }

    /// <summary>
    /// Sets the correct rotations for both picks based on the current pick and the random values
    /// </summary>
    private void SetCorrectLockRotations()
    {
        correctRotationLeft = randomValues[currentLock * 2];
        correctRotationRight = randomValues[(currentLock * 2) + 1];
    }

    private void OnDrawGizmosSelected()
    {
        // Draws lines showing the direction of the picks that equate to the correct positions to open the chest
        Vector3 left = pickPosition.TransformPoint(new Vector3(Mathf.Sin(correctRotationLeft * Mathf.Deg2Rad), Mathf.Cos(correctRotationLeft * Mathf.Deg2Rad)) * 0.1f);
        Vector3 right = pickPosition.TransformPoint(new Vector3(Mathf.Sin(correctRotationRight * Mathf.Deg2Rad), Mathf.Cos(correctRotationRight * Mathf.Deg2Rad)) * 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pickPosition.position, left);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pickPosition.position, right);
    }
}