using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalTimer
{
    public static double startingTime = 0f;

    public static bool hasStarted;

    private static float time = 0f;

    /// <summary>
    /// Returns the time from the starting time in decimals
    /// </summary>
    public static float Time
    {
        get
        {
            if (!hasStarted)
            {
                return time;
            }
            else
            {
                return (float)(UnityEngine.Time.timeAsDouble - startingTime);
            }
        }
    }

    public static void StartTimer()
    {
        hasStarted = true;
        startingTime = UnityEngine.Time.timeAsDouble;
        EventManager.onPlayerTimerStart.Invoke();
    }

    public static void StopTimer()
    {
        time = Time;
        hasStarted = false;
    }

    public static void ResetTimer()
    {
        time = 0f;
    }
}
