using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;
    public UnityEvent<GameObject> onGameObjectEnter;
    public UnityEvent<GameObject> onGameObjectExit;

    public LayerMask layerMask = ~0;
    public string objectTag = "";

    private void OnTriggerEnter(Collider other)
    {
        if ((objectTag == "" && layerMask.Contains(other.gameObject.layer)) || (objectTag != "" && other.CompareTag(objectTag)))
        {
            onEnter.Invoke();
            onGameObjectEnter.Invoke(other.gameObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if ((objectTag == "" && layerMask.Contains(other.gameObject.layer)) || (objectTag != "" && other.CompareTag(objectTag)))
        {
            onExit.Invoke();
            onGameObjectExit.Invoke(other.gameObject);
        }
    }
}
