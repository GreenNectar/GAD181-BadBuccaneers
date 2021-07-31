using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEvents : MonoBehaviour
{
    [SerializeField]
    private List<UnityEvent> events = new List<UnityEvent>();

    public void InvokeEvent(int eventId)
    {
        events[eventId].Invoke();
    }
}
