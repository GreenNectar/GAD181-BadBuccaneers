using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Player wins");
           //log = debug.log string v = "player wins";
        }
    }
}
