using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimerController : MonoBehaviour
{
    [SerializeField]
    private GameObject timer;

    [SerializeField]
    private TextMeshProUGUI time;

    [SerializeField, TextArea]
    private string timeText;

    private void OnEnable()
    {
        EventManager.onTimerEnd.AddListener(HideTimer);
        EventManager.onTimerStop.AddListener(HideTimer);
        EventManager.onTimerStart.AddListener(ShowTimer);
    }

    private void OnDisable()
    {
        EventManager.onTimerStart.RemoveListener(ShowTimer);
        EventManager.onTimerEnd.RemoveListener(HideTimer);
        EventManager.onTimerStop.RemoveListener(HideTimer);
        HideTimer();
    }

    private void Update()
    {
        if (TimeManager.Instance.isTiming)
        {
            ShowTimer();
        }
    }

    private void ShowTimer()
    {
        SetTimeText();
        timer.SetActive(true);
    }

    private void HideTimer()
    {
        if (timer)
            timer.SetActive(false);
    }

    private void SetTimeText()
    {
        time.text = timeText + TimeManager.Instance.TimeText;
    }
}
