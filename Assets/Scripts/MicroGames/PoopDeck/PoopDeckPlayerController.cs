using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PoopDeckPlayerController : MicroGamePlayerController
{
    private CharacterController characterController;

    [SerializeField]
    private float speed = 1f;
    [SerializeField, Tooltip("Degrees per second")]
    private float rotationSpeed = 90f;
    [SerializeField]
    private Transform mop;

    //! REMOVE
    [SerializeField]
    private int score;

    protected override void Start()
    {
        base.Start();

        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input
        Vector3 movement = new Vector3(player.GetAxis("LeftMoveX"), 0f, player.GetAxis("LeftMoveY"));

        // Movement, we only want to do it if we are pressing a button
        if (movement.sqrMagnitude > 0f)
        {
            // Get the rotations
            Quaternion wantedRotation = Quaternion.LookRotation(movement, Vector3.up); // Movement direction
            Quaternion currentRotation = transform.rotation;
            Vector3 currentEuler = currentRotation.eulerAngles; // Get the euler so I don't have to write so much

            // Get the difference in angle
            float difference = Mathf.DeltaAngle(currentEuler.y, wantedRotation.eulerAngles.y);

            // Move towards the target angle, this is basically lerping between the current to the wanted
            if (Mathf.Abs(difference) > rotationSpeed * Time.deltaTime)
            {
                if (difference > 0)
                {
                    currentEuler.y += rotationSpeed * Time.deltaTime;
                }
                else
                {
                    currentEuler.y -= rotationSpeed * Time.deltaTime;
                }
            }
            else
            {
                currentEuler.y = wantedRotation.eulerAngles.y;
            }

            // Set the rotation
            transform.rotation = Quaternion.Euler(currentEuler);
        }

        // Move the player in the direction we are looking towards
        characterController.Move(transform.forward * Time.deltaTime * speed * movement.magnitude);
    }

    private void FixedUpdate()
    {
        // We don't want to keep mopping more than we have to
        Ray ray = new Ray { origin = mop.position, direction = Vector3.down };

        score += PoopDeckTesting.current.Mop(ray);
    }
}
