using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{ 

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Collison");

        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
        }

    }

}
