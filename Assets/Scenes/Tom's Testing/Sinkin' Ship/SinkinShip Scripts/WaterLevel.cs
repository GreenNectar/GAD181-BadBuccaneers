using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{ 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerCanyonClimber>().Drown();
        }
    }
}
