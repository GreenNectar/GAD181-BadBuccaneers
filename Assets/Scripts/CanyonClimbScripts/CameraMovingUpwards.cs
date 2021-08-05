using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovingUpwards : MonoBehaviour
{

    public CharacterController cam;
    public float camSpeed;
    private Vector3 Vec3 = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        cam.Move(Vec3 * Time.deltaTime);
        cam.Move(Vector3.up * Time.deltaTime * camSpeed);

    }
}
