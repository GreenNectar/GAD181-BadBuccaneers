using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimedPlayerPanel : PlayerPanelController
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    public float time;
    private bool isTiming;

    private void OnEnable()
    {
        EventManager.onTimerStart.AddListener(StartTime);
        EventManager.onPlayerFinish.AddListener(StopTime);
    }

    private void OnDisable()
    {
        EventManager.onTimerStart.RemoveListener(StartTime);
        EventManager.onPlayerFinish.RemoveListener(StopTime);
    }

    public void StartTimer()
    {
        GlobalTimer.StartTimer();
    }

    public void StartTime()
    {
        isTiming = true;
    }

    private void Update()
    {
        if (isTiming)
        {
            time = GlobalTimer.Time;
            UpdateUI();
        }
    }

    public void StopTimer()
    {
        EventManager.onPlayerFinish.Invoke(PlayerNumber);
    }

    public void StopTime(int player)
    {
        if (player == PlayerNumber)
        {
            time = GlobalTimer.Time;
            isTiming = false;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        int milliseconds = (int)((time % 1f) * 1000f);
        int seconds = (int)(time % 60f);
        int minutes = (int)(time / 60f);
        timerText.text =$"{minutes}:{seconds.ToString("D2")}:{milliseconds.ToString("D3").Substring(0, 2)}";
    }
}
