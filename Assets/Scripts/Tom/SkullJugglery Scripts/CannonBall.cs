using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{

    public float frontBarrier;
    public float backBarrier;

    void Update()
    {
        // This clamps the cannonball on the z axis 
        Vector3 clampedPosition = transform.position;
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, frontBarrier, backBarrier);
        transform.position = clampedPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bucket"))
        {
            Destroy(gameObject);
        }
    }
}
