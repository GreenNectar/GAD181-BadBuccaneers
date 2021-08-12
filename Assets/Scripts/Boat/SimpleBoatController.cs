using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Buoyancy))]
[RequireComponent(typeof(Rigidbody))]
public class SimpleBoatController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 15f;
    [SerializeField]
    private float turnSpeed = 1f;
    [SerializeField]
    private float maxmimumVelocity = 10f;

    private Buoyancy buoyancy;
    private Rigidbody rigidbody;

    [SerializeField]
    private Vector3[] positions = new Vector3[1];


    // Start is called before the first frame update
    void Start()
    {
        buoyancy = GetComponent<Buoyancy>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // We don't want drag to affect the direction we want to be moving
        buoyancy.draglessDirection = new Vector3(move.x, 0f, -1f);

        // Movement using buoyant points
        //foreach (var position in buoyancy.ForcePositions)
        //{
        //    if (position.isSubmersed)
        //    {
        //        rigidbody.AddForceAtPosition(transform.forward * move.z * Time.deltaTime * movementSpeed / buoyancy.ForcePositions.Length, transform.TransformPoint(position.position), ForceMode.Impulse);
        //        rigidbody.AddTorque(transform.up * move.x * Time.deltaTime * turnSpeed / buoyancy.ForcePositions.Length, ForceMode.Impulse);
        //    }
        //}

        // Movement using custom points
        foreach (var p in positions)
        {
            // Forward and backward movement
            rigidbody.AddForceAtPosition((transform.forward * move.z * Time.deltaTime * movementSpeed / positions.Length) * buoyancy.SubmersedPercentage, transform.TransformPoint(p), ForceMode.VelocityChange);

            // Left and right turning
            rigidbody.AddTorque((transform.up * move.x * Time.deltaTime * turnSpeed / positions.Length) * buoyancy.SubmersedPercentage, ForceMode.VelocityChange);
        }

        // Clamp the velocity
        if (rigidbody.velocity.magnitude > maxmimumVelocity)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxmimumVelocity;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Show each point as a red sphere
        Gizmos.color = Color.red;
        foreach (var p in positions)
        {
            Gizmos.DrawSphere(transform.TransformPoint(p), 0.1f);
        }
    }
}
