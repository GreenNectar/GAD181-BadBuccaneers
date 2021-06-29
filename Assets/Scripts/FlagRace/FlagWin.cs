using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagWin : MonoBehaviour
{

    // Once the player enters the trigger zone, prints "player wins"
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Player wins");
         
        }
    }
}
