using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : Singleton<TimeManager>
{
    public float currentTime { get; private set; }
    public bool isTiming { get; private set; } = false;

    public string TimeText
    {
        get
        {
            return Mathf.CeilToInt(currentTime).ToString();//((int)(currentTime - (currentTime % 1f))).ToString();
        }
    }

    public static void StartTimer(float time)
    {
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(Instance.CountDownTimer(time));
        Instance.isTiming = true;
        EventManager.onTimerStart.Invoke();
    }

    public static void StopTimer()
    {
        if (Instance.isTiming)
        {
            Instance.StopAllCoroutines();
            Instance.isTiming = false;
        }
        EventManager.onTimerStop.Invoke();
    }

    private IEnumerator CountDownTimer(float time)
    {
        currentTime = time;
        isTiming = true;
        while (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0f, float.MaxValue);
            yield return null;
        }
        isTiming = false;
        EventManager.onTimerEnd.Invoke();
    }
}
