using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Registry
{
    private List<Object> registeredObjects = new List<Object>();
    private int previousCount = 0;
    //private bool firstcall = true;

    public UnityEvent onOccupied = new UnityEvent();
    public UnityEvent onUnOccupied = new UnityEvent();

    public bool Occupied
    {
        get
        {
            return registeredObjects.Count > 0;
        }
    }

    public void Register(Object obj)
    {
        if (registeredObjects.Count == 0) OnOccupied();

        if (!registeredObjects.Contains(obj)) registeredObjects.Add(obj);
    }

    public void UnRegister(Object obj)
    {
        previousCount = registeredObjects.Count;
        if (registeredObjects.Contains(obj)) registeredObjects.Remove(obj);

        if (previousCount != registeredObjects.Count && registeredObjects.Count == 0) OnUnOccupied();
        //if (firstcall && registeredObjects.Count == 0) { OnUnOccupied(); firstcall = false; }
    }

    public void Clear()
    {
        registeredObjects.Clear();
        OnUnOccupied();
    }

    internal void OnOccupied()
    {
        onOccupied.Invoke();
    }

    internal void OnUnOccupied()
    {
        onUnOccupied.Invoke();
    }
}
