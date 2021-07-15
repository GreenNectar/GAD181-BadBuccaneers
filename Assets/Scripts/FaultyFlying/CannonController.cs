using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    // Cannon Rotation Variables
    public float speed = 15f; // speed at which cannon can be turned
    public float friction = 1f; // resistance against turning
    public float lerpspeed = 1f;
    public float gravity = 4f; // speed at which cannon automatically tilts back down
    public float fuseTime = 1f; //delay on 
    float vertical;
    float horizontal;
    Vector2 verticalRange = new Vector2(-60f, 10f);
    Vector2 horizontalRange = new Vector2(-45f, 45f);
    public Transform barrel;
    public GameObject fuse;


    Camera camera;

    // Cannon Firing Variable
    public GameObject flyingPirate;
    Rigidbody flyingPirateRB;
    public Transform shotPos;
    public Transform explosionPos;
    public GameObject explosion;
    public GameObject loadedMan;
    public float firePower;
    private bool hasFired;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;

    }

    // Controls where we are aiming our cannon
    void Update()
    {
        // Cannon tilt
        vertical += Time.deltaTime * gravity;

        // Directing cannon controls
        vertical -= Input.GetAxis("Vertical") * speed * friction * Time.deltaTime;
        horizontal += Input.GetAxis("Horizontal") * speed * friction * Time.deltaTime;

        // Change cannon rotation
        vertical = Mathf.Clamp(vertical, verticalRange.x, verticalRange.y);
        horizontal = Mathf.Clamp(horizontal, horizontalRange.x, horizontalRange.y);
        Quaternion fromRotation = transform.rotation;
        Quaternion toRotation = Quaternion.Euler(0f, horizontal, 0f);
        Quaternion barrelToRotation = Quaternion.Euler(vertical, 0f, 0f);
        transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpspeed);
        barrel.rotation = Quaternion.Lerp(barrel.rotation, barrelToRotation, Time.deltaTime * lerpspeed);


        // Firing controls ensures cannon cannot be fired a second time & the man in the cannon disappears upon firing
        if (Input.GetKeyDown(KeyCode.Space) && !hasFired)
        {
            StartCoroutine(FireSequence());
            hasFired = true;
        }
    }

    // Add delay to fire
    private IEnumerator FireSequence()
    {
        fuse.SetActive(true); 
        yield return new WaitForSeconds(fuseTime);
        FireCannon();
        fuse.SetActive(false);
    }

    // For shooting the cannon
    private void FireCannon()
    {
        shotPos = loadedMan.transform;//rotation = transform.rotation;
        GameObject flyingPirateCopy = Instantiate(flyingPirate, shotPos.position, shotPos.rotation) as GameObject;
        Destroy(loadedMan);
        flyingPirateRB = flyingPirateCopy.GetComponent<Rigidbody>();
        flyingPirateRB.AddForce(transform.forward * firePower * 100f);
        Instantiate(explosion, explosionPos.position, explosionPos.rotation);
    }

}
