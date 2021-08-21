using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float time = 30f;

    [SerializeField]
    private bool onStart = true;

    [SerializeField]
    private bool finishLevelOnComplete = false;

    [SerializeField]
    private bool stopTimerOnPlayersFinished = false;

    public UnityEvent onTimerFinish;

    public bool IsTiming => TimeManager.Instance.isTiming;

    private void OnEnable()
    {
        EventManager.onTimerEnd.AddListener(TimerEnd);
        EventManager.onPlayerFinish.AddListener(CheckPlayersFinish);
    }

    private void OnDisable()
    {
        EventManager.onTimerEnd.RemoveListener(TimerEnd);
        EventManager.onPlayerFinish.RemoveListener(CheckPlayersFinish);
    }

    private void Start()
    {
        if (onStart)
        {
            StartTimer(true);
        }
    }

    public void StartTimer(bool forceStart = false)
    {
        if (forceStart || (!forceStart && !TimeManager.Instance.isTiming))
            TimeManager.StartTimer(time);
    }

    private void TimerEnd()
    {
        if (finishLevelOnComplete)
        {
            GameManager.EndGameStatic();
        }

        onTimerFinish.Invoke();
    }

    private void CheckPlayersFinish()
    {
        if (stopTimerOnPlayersFinished)
        {
            if (ScoreManager.Instance.AllPlayersFinished)
            {
                TimeManager.StopTimer();
                if (finishLevelOnComplete) GameManager.EndGameStatic();
            }
        }
    }
}