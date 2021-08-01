using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMoveTo : MonoBehaviour
{
    public void MoveTo(Transform transform)
    {
        base.transform.position = transform.position;
    }
}
