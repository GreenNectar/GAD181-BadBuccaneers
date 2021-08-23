using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovesForward : MonoBehaviour
{

    //public float forwardSpeed = 2f;
    //public float forwardMax = 50f;
    //public float time = 3f;




    //// Update is called once per frame
    //void Update()
    //{
    //    if (time >= 0)
    //    {
    //        time -= Time.deltaTime;
    //        return;
    //    }
    //    else
    //    {
    //        transform.Translate(Vector3.up * Time.deltaTime * forwardSpeed);
    //        Vector3 clampedPosition = transform.position;
    //        clampedPosition.z = Mathf.Clamp(clampedPosition.z, 0f, forwardMax);
    //        transform.position = clampedPosition;
    //    }

    //}

    [SerializeField]
    private Transform start;
    [SerializeField]
    private Transform end;
    [SerializeField]
    private float time = 1f;
    [SerializeField]
    private bool rotate = false;

    private float currentTime;


    private void Start()
    {
        transform.position = start.position;
        if (rotate) transform.rotation = start.rotation;
    }

    private void Update()
    {
        if (currentTime < time)
        {
            currentTime += Time.deltaTime;
        }

        currentTime = Mathf.Clamp(currentTime, 0f, time);

        transform.position = Vector3.Lerp(start.position, end.position, currentTime / time);
        if (rotate) transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, currentTime / time);
    }
}
