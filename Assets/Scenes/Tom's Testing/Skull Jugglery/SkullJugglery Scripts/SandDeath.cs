using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDeath : MonoBehaviour
{

    public Transform cannonBallLocation;
    public GameObject cannonBallSpawn;

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Collison");

        if (other.gameObject.tag == "CannonBall")
        {
            Destroy(other.gameObject);
            Instantiate(cannonBallSpawn, transform.position, transform.rotation);
        }

    }

}
