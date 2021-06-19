using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class LockpickController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private Transform pickLeft;
    [SerializeField]
    private Transform pickRight;
    [SerializeField]
    private Transform pickPosition;

    [SerializeField]
    private float correctRotationLeft = 0f;
    [SerializeField]
    private float correctRotationRight = 180f;
    [SerializeField]
    private float correctDistance = 15f;
    [SerializeField]
    private float vibrationDistance = 30f;

    [SerializeField]
    private float rotationLeft = 0f;
    [SerializeField]
    private float rotationRight = 0f;
    [SerializeField]
    private float currentRotationLeft = 0f;
    [SerializeField]
    private float currentRotationRight = 0f;

    private PlayerInput playerInput;
    private Animator animator;

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

    public void OnLeftMove(InputValue value)
    {
        if (value.Get<Vector2>().sqrMagnitude > 0.25f)
        {
            rotationLeft = value.Get<Vector2>().Angle();
        }
    }

    public void OnRightMove(InputValue value)
    {
        if (value.Get<Vector2>().sqrMagnitude > 0.25f)
        {
            rotationRight = value.Get<Vector2>().Angle();
        }
    }

    public void OnFire(InputValue value)
    {
        // Check if we are allowed to open
        if (!canOpen) return;

        // If we have pressed the button, then we check to see if we have fulfilled the conditions and then trigger the animator to open the chest
        if (value.isPressed)
        {
            if (DistanceFromCorrect <= correctDistance)
            {
                animator.SetTrigger("Open");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position += Vector3.left * 50f * (FindObjectsOfType<PlayerInput>(false).Length - 1f);

        playerInput = GetComponentInParent<PlayerInput>();
        animator = GetComponent<Animator>();
        currentRotationLeft = rotationLeft;
        currentRotationRight = rotationRight;
    }

    // Update is called once per frame
    void Update()
    {
        var gamePad = Gamepad.all.FirstOrDefault(g => playerInput.devices.Any(d => d.deviceId == g.deviceId));

        if (!canOpen)
        {
            if (gamePad != null)
            {
                gamePad.SetMotorSpeeds(0f, 0f);
                return;
            }
        }
        else
        {
            if (gamePad != null)
            {
                // This is the vibration that has to have both of the picks in the correct rotation (with 'vibrationDistance' maximum degrees between them)
                // The vibration is scaled to how close it is to the correct positions
                if (DistanceFromCorrect <= vibrationDistance) // If the difference is lower than the vibration distance, we want to set the vibration
                {
                    // The vibration amount is the inverse distance from the max distance over the distance. This will turn (0 to vibrationDistance degrees to 0 to 1)
                    float vibrationAmount = (vibrationDistance - DistanceFromCorrect) / vibrationDistance;
                    vibrationAmount *= vibrationAmount; // Square it, this makes the transitions feel a bit nicer
                    gamePad.SetMotorSpeeds(vibrationAmount, vibrationAmount); // Set it brother
                                                                              //Debug.Log($"Vibration amount l:{vibrationAmount} r:{vibrationAmount}");
                }
                else
                {
                    gamePad.SetMotorSpeeds(0f, 0f);
                    //Debug.Log($"Vibration amount l:{0f} r:{0f}");
                }
            }

            // Move to the wanted rotation with the maximum step being delta time by speed (degrees per second)
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

    private void OnDisable()
    {
        DisableVibrations();
    }

    private void OnDestroy()
    {
        DisableVibrations();
    }

    private void DisableVibrations()
    {
        // Get the gamepad associated with the player
        //TODO Don't do this everytime
        var gamePad = Gamepad.all.FirstOrDefault(g => playerInput.devices.Any(d => d.deviceId == g.deviceId));
        if (gamePad != null)
        {
            gamePad.SetMotorSpeeds(0f, 0f);
            gamePad.ResetHaptics();
        }
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