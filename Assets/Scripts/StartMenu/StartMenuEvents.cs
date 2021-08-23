using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartMenuEvents : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;

    public virtual void Enter()
    {
        onEnter.Invoke();
    }

    public virtual void Exit()
    {
        onExit.Invoke();
    }
}
