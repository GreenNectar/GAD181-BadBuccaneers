using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mathfx
{
    /// <summary>
    /// Remaps a float from two values, to another two values
    /// </summary>
    /// <param name="value"></param>
    /// <param name="fromMin"></param>
    /// <param name="fromMax"></param>
    /// <param name="toMin"></param>
    /// <param name="toMax"></param>
    /// <returns></returns>
    public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}
