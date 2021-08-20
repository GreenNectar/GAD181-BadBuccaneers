using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    //[SerializeField, Scene]
    //private string scoreScene;

    public static UnityEvent<int> onPlayerFinish = new UnityEvent<int>();

    //public static UnityEvent<int, int> onPlayerScore = new UnityEvent<int, int>();
   

    public static UnityEvent onResultsFinish = new UnityEvent();

    public static UnityEvent onTimerStart = new UnityEvent();
    public static UnityEvent onTimerEnd = new UnityEvent();
    public static UnityEvent onTimerStop = new UnityEvent();
    public static UnityEvent onPlayerTimerStart = new UnityEvent();

    public static UnityEvent onGameEnd = new UnityEvent();

    #region Scoring Type Events

    public static UnityEvent<int> onUpdateScore = new UnityEvent<int>();
    //public static UnityEvent<int> onElimination = new UnityEvent<int>();

    #endregion
    
    public static void FinishResults()
    {
        onResultsFinish.Invoke();
    }
}