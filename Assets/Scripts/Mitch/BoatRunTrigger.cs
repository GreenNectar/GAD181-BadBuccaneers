using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRunTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   public void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GetComponent<BoatRunController>().inBooster = true;
        }
       
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<BoatRunController>().inBooster = false;
        }
    }
}
