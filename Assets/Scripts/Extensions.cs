using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static bool Contains(this LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    public static float Angle(this Vector2 vector)
    {
        if (vector.x < 0)
        {
            return 360 - (Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
        }
    }

    public static float Step(this float value, float max, int steps)
    {
        return Mathf.Round(value / (max / steps)) * (max / steps);
    }

    public static float StepCeil(this float value, float max, int steps)
    {
        return Mathf.Ceil(value / (max / steps)) * (max / steps);
    }

    public static float StepFloor(this float value, float max, int steps)
    {
        return Mathf.Floor(value / (max / steps)) * (max / steps);
    }

    /// <summary>
    /// Performs an action on all array elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T item in source)
            action(item);
    }

    public static string Ordinal(this int number)
    {
        string[] ordinals = { "umm", "st", "nd", "rd", "th" };
        string ordinal = "bruh";

        if (number == 0)
        {
            return "";
        }

        if (number <= 20)
        {
            ordinal = number.ToString() + ordinals[Mathf.Clamp(number, 0, 4)];
        }
        else
        {
            switch (number % 10)
            {
                case 1:
                    ordinal = number.ToString() + ordinals[1];
                    break;
                case 2:
                    ordinal = number.ToString() + ordinals[2];
                    break;
                case 3:
                    ordinal = number.ToString() + ordinals[3];
                    break;
                default:
                    ordinal = number.ToString() + ordinals[4];
                    break;
            }

            //ordinal = number.ToString() + ordinals[Mathf.Clamp((number % 10), 1, 4)];
        }

        return ordinal;
    }
}
