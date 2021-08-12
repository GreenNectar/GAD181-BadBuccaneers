using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    public GameObject followObj;
    [Range(0f, 1f)]
    public float lerpAmount;
    public float cameraDistance;
    public float cameraOffestY;
    float newX;
    float newY;

    void LateUpdate()
    {  
         float newX2 = followObj.transform.position.x; //Follows the players position
         float newY2 = followObj.transform.position.y;

         newX2 = Mathf.Lerp(newX2, transform.position.x, lerpAmount);
         newY2 = Mathf.Lerp(newY2, transform.position.y, lerpAmount);
         transform.position = Vector3.Lerp(new Vector3(newX2, newY2, cameraDistance),transform.position,lerpAmount);

    }

}
