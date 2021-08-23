using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Line : MonoBehaviour
{
    [SerializeField]
    private GameObject hookedObject;

    public UnityEvent<GameObject> onHook;
    public UnityEvent<GameObject> onUnHook;

    private Quaternion startingRotation;

    public void HookObject(GameObject obj)
    {
        if (hookedObject) return;

        if (obj.GetComponent<Fish>())
        {
            obj.GetComponent<Fish>().enabled = false;
        }

        startingRotation = obj.transform.localRotation;
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

        hookedObject = obj;
        onHook.Invoke(obj);
    }

    public void UnHook(bool isLetGo)
    {
        if (!hookedObject) return;

        GameObject temp = hookedObject;
        hookedObject.transform.parent = null;
        hookedObject = null;

        if (isLetGo)
        {
            temp.transform.localRotation = startingRotation;
            if (temp.GetComponent<Fish>())
            {
                temp.GetComponent<Fish>().enabled = true;
            }
        }

        onUnHook.Invoke(temp);
    }
}
