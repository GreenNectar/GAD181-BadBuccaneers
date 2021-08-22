using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRunController : MicroGamePlayerController
{
    [SerializeField]
    private float turnSpeed = 30f;
    [SerializeField]
    private float turnForce = 0.1f;
    [SerializeField]
    private float maxTurn = 30f;
    [SerializeField]
    private float boatSpeed = 1f;
    [SerializeField]
    private float boostSpeed = 2f;
    [SerializeField]
    private float speedLerpSpeed = 1f;
    [SerializeField]
    private Transform boat;
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private Buoyancy buoyancy;
    [SerializeField]
    private CameraFollower cameraFollower;
    //[SerializeField]
    //private float boatLean = 30f;
    //[SerializeField]
    //private float leanLerpSpeed = 1f;


    private bool inBooster;
    float currentSpeed;
    float wantedSpeed;
    float currentTurn;
    bool canMove = true;


    public void StartTimer()
    {
        ScoreManager.Instance.StartTimer();
    }

    public void EndTimer()
    {
        ScoreManager.Instance.EndPlayer(PlayerNumber);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentSpeed = 0f;
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        // Booster...
        if (inBooster == true)
        {
            wantedSpeed = boostSpeed;
        }
        else
        {
            wantedSpeed = boatSpeed;
        }

        // Lerp the speed
        float speedDelta = Time.deltaTime * speedLerpSpeed;
        if (Mathf.Abs(wantedSpeed - currentSpeed) > speedDelta)
        {
            if (wantedSpeed > currentSpeed)
            {
                currentSpeed += speedDelta;
            }
            else
            {
                currentSpeed -= speedDelta;
            }
        }
        else
        {
            currentSpeed = wantedSpeed;
        }

        // Move the player in the direction they inputted (0.5 is to make the influence less)
        Vector3 force = new Vector3(player.GetAxis("LeftMoveX") * 0.5f * (canMove ? 1f : 0f),  0f, 1f);
        force.Normalize();// = force.normalized * Mathf.Clamp(force.magnitude, 0f, 1f);
        buoyancy.draglessDirection = force; // So there's no drag in the direction we want to move
        rigidBody.AddForceAtPosition(force * buoyancy.SubmersedPercentage * currentSpeed * Time.deltaTime, transform.position + transform.forward * 5, ForceMode.Acceleration);

        // Lerp the turn/rotation
        float wantedTurn = player.GetAxis("LeftMoveX") * maxTurn * (canMove ? 1f : 0f);
        float turnDelta = Time.deltaTime * turnSpeed;
        if (Mathf.Abs(Mathf.DeltaAngle(wantedTurn, currentTurn)) > turnDelta)
        {
            if (currentTurn < wantedTurn)
            {
                currentTurn += turnDelta;
            }
            else
            {
                currentTurn -= turnDelta;
            }
        }
        else
        {
            currentTurn = wantedTurn;
        }

        // Add the rotation force to aim towards where we want
        float angleDelta = Mathf.DeltaAngle(currentTurn, transform.localEulerAngles.y);
        if (Mathf.Abs(angleDelta) > 5f)
        {
            if (angleDelta < 0)
            {
                rigidBody.AddTorque(transform.up * turnForce * Time.deltaTime, ForceMode.Acceleration);
            }
            else
            {
                rigidBody.AddTorque(transform.up * -turnForce * Time.deltaTime, ForceMode.Acceleration);
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(CollisionSequence(collision.contacts[0].point, collision.contacts[0].normal));
    //}

    //IEnumerator CollisionSequence(Vector3 hitPoint, Vector3 normal)
    //{
    //    currentSpeed = 0f;
    //    rigidBody.AddForce(hitPoint - transform.position, );
    //    yield return new WaitForSeconds(1f);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Booster")
        {
            inBooster = true;
        }
        if (other.tag == "FinishLine")
        {
            canMove = false;
            cameraFollower.enabled = false;
            EndTimer();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Booster")
        {
            inBooster = false;
        }
    }
}
