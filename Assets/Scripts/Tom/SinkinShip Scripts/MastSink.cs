using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastSink : MonoBehaviour
{

    public float downSpeed = 2f;
 

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * downSpeed);
    }
}
