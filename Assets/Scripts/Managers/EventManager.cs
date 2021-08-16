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

    public static UnityEvent<float> onTimerStart = new UnityEvent<float>();
    public static UnityEvent onTimerEnd = new UnityEvent();
    public static UnityEvent onTimerStop = new UnityEvent();
    public static UnityEvent onPlayerTimerStart = new UnityEvent();

    public static UnityEvent onGameEnd = new UnityEvent();

    private static bool isTiming = false;

    #region Scoring Type Events

    public static UnityEvent<int> onUpdateScore = new UnityEvent<int>();
    //public static UnityEvent<int> onElimination = new UnityEvent<int>();

    #endregion

    #region Microgame Time
    public static void StartTimer(float time)
    {
        onTimerStart.Invoke(time);
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(CountDownTimer(time));
    }

    public static void StopTimer()
    {
        onTimerStop.Invoke();
        if (isTiming)
        {
            Instance.StopAllCoroutines();
        }
    }

    private static IEnumerator CountDownTimer(float time)
    {
        isTiming = true;
        yield return new WaitForSeconds(time);
        isTiming = false;
        onTimerEnd.Invoke();
    }
    #endregion

    #region Player Time

    #endregion
    
    public static void FinishResults()
    {
        onResultsFinish.Invoke();
    }
}