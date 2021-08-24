using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDebris : MonoBehaviour
{
    public float downSpeed = 2f;
    public float destroyTime = 5f;


    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * downSpeed);
        Destroy(gameObject, destroyTime);
    }

    
}
