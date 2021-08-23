using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveRaceCam : MonoBehaviour
{
    public CharacterController cam;
    public float camSpeed;
    private Vector3 Vec3 = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.Move(Vec3 * Time.deltaTime);
        cam.Move(Vector3.forward * Time.deltaTime * camSpeed);

    }
}
