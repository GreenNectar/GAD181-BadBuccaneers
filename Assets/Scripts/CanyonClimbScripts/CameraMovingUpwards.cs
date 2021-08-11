using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovingUpwards : MonoBehaviour
{

    public float upSpeed = 2f;
    public float topHeight = 50f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * upSpeed);

        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0f, topHeight);
        transform.position = clampedPosition;
    }

    
}
