using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Attach this script to an object that has a trigger collider.
//When another object enters the trigger collider this will call the trigger enter event 
//When another object enters the trigger collider this will call the trigger exit event 
// Make a copy of this script and change the tag to implement different reactions to different objects. 




public class PlayerEnterOrExitTriggerZoneEvent : MonoBehaviour
{
    public UnityEvent triggerEnterEvent;

    public UnityEvent triggerExitEvent;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            triggerEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            triggerExitEvent.Invoke();
        }
    }
}
