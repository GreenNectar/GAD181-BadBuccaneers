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
}
