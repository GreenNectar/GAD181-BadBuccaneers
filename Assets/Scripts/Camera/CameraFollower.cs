using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private bool keepOriginalOffset;

    [SerializeField]
    private bool followPosition;
    [SerializeField]
    private bool followRotation;

    [SerializeField]
    private float positionLerpSpeed;
    [SerializeField]
    private float rotationLerpSpeed;


    Vector3 offset = Vector3.zero;


    private void Start()
    {
        if (keepOriginalOffset)
        {
            offset = target.InverseTransformPoint(transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (followPosition)
            {
                transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * positionLerpSpeed);
            }
            if (followRotation)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotationLerpSpeed);
            }
        }
    }
}
