using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    //[SerializeField]
    //public float farRight;
    //public float farLeft;
    public float frontBarrier;
    public float backBarrier;

    void Update()
    {
        // This clamps the cannonball on the z axis 
        Vector3 clampedPosition = transform.position;
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, frontBarrier, backBarrier);
        transform.position = clampedPosition;
    }
}
