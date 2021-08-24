using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalTimer
{
    public static double startingTime = 0f;

    /// <summary>
    /// Returns the time from the starting time in decimals
    /// </summary>
    public static float Time
    {
        get
        {
            return (float)(UnityEngine.Time.timeAsDouble - startingTime);
        }
    }

    public static void StartTimer()
    {
        startingTime = UnityEngine.Time.timeAsDouble;
        EventManager.onTimerStart.Invoke();
    }
}
