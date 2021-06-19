using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private float correctRotationLeft = 0f;
    private float correctRotationRight = 180f;
    private float maxVibration = 1f;
    private float vibrationDistance = 20f;

    private float rotationLeft = 0f;
    private float rotationRight = 0f;
    private float currentRotationLeft = 0f;
    private float currentRotationRight = 0f;

    private PlayerInput playerInput;

    public void OnLeftMove(InputValue value)
    {
        if (value.Get<Vector2>().sqrMagnitude > 0.25f)
        {
            rotationLeft = value.Get<Vector2>().Angle(); //Vector2.Angle(Vector2.zero, value.Get<Vector2>().normalized);
        }
    }

    public void OnRightMove(InputValue value)
    {
        if (value.Get<Vector2>().sqrMagnitude > 0.25f)
        {
            rotationRight = value.Get<Vector2>().Angle(); //Vector2.Angle(Vector2.zero, value.Get<Vector2>().normalized);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        var gamePad = Gamepad.all.FirstOrDefault(g => playerInput.devices.Any(d => d.deviceId == g.deviceId));
        if (gamePad != null)
        {
            // This is the vibration that has to have both of the picks in the correct rotation (with 'vibrationDistance' maximum degrees between them)
            // The vibration is scaled to how close it is to the correct positions
            float leftDiff = Mathf.Abs(Mathf.DeltaAngle(currentRotationLeft, correctRotationLeft)); // Get the angle distance of the left pick
            float rightDiff = Mathf.Abs(Mathf.DeltaAngle(currentRotationRight, correctRotationRight)); // Get the angle distance of the right pick
            float diff = leftDiff + rightDiff; // Add the differences
            if (diff <= vibrationDistance) // If the difference is lower than the vibration distance, we want to set the vibration
            {
                // The vibration amount is the inverse distance from the max distance over the distance. This will turn (0 to vibrationDistance degrees to 0 to 1)
                float vibrationAmount = (vibrationDistance - diff) / vibrationDistance;
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

        // Move to the wanted rotation with the maximum step being delta time by speed (degrees per second)
        currentRotationRight += Mathf.Clamp(Mathf.DeltaAngle(currentRotationRight, rotationRight), -Time.deltaTime * speed, Time.deltaTime * speed);
        currentRotationLeft += Mathf.Clamp(Mathf.DeltaAngle(currentRotationLeft, rotationLeft), -Time.deltaTime * speed, Time.deltaTime * speed);

        // Set the rotation
        pickRight.rotation = Quaternion.Euler(0, 0, currentRotationRight);
        pickLeft.rotation = Quaternion.Euler(0, 0, currentRotationLeft);
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
        var gamePad = Gamepad.all.FirstOrDefault(g => playerInput.devices.Any(d => d.deviceId == g.deviceId));
        gamePad.SetMotorSpeeds(0f, 0f);
    }
}