using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformKeepPosition : MonoBehaviour
{
    [SerializeField]
    private bool lockX, lockY, lockZ;

    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(
            lockX ? startingPosition.x : transform.position.x,
            lockY ? startingPosition.y : transform.position.y,
            lockZ ? startingPosition.z : transform.position.z);
    }
}
