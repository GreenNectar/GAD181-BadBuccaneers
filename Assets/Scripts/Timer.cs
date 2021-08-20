using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float time = 30f;

    [SerializeField]
    private bool onStart = true;

    [SerializeField]
    private bool finishLevelOnComplete = false;

    private void OnEnable()
    {
        if (finishLevelOnComplete) EventManager.onTimerEnd.AddListener(EndGame);
    }

    private void OnDisable()
    {
        if (finishLevelOnComplete) EventManager.onTimerEnd.RemoveListener(EndGame);
    }

    private void Start()
    {
        if (onStart)
        {
            StartTimer();
        }
    }

    public void StartTimer()
    {
        TimeManager.StartTimer(time);
    }

    private void EndGame()
    {
        GameManager.EndGameStatic();
    }
}
