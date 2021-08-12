using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField]
    public float farRight;
    public float farLeft;
    public float frontBarrier;
    public float backBarrier;


    // Update is called once per frame
    void Update()
    {
        // This clamps the flag on the x axis
        Vector3 clampedPosition = transform.position;
        //clampedPosition.z = Mathf.Clamp(clampedPosition.z, farLeft, farRight);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, frontBarrier, backBarrier);
        transform.position = clampedPosition;
    }
}
