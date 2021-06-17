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
}
