using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class WaterLevelFinder : MonoBehaviour
{

    protected static float _Time
    {
        get
        {
#if UNITY_EDITOR
            return Application.isPlaying ? Time.time : Shader.GetGlobalVector("_Time").y;
#else
                return Time.time;
#endif
        }
    }

    public Vector3 GetWaterSurfacePosition()
    {
        return GetWaterSurfacePosition(transform.position);
    }

    public virtual Vector3 GetWaterSurfacePosition(Vector3 position)
    {
        return Vector3.zero;
    }

    public virtual Vector3 GetWaterNormal(Vector3 position)
    {
        Vector3 a = GetWaterSurfacePosition(position + new Vector3(0.005f, 0f, 0.005f));
        Vector3 b = GetWaterSurfacePosition(position + new Vector3(0.005f, 0f, -0.005f));
        Vector3 c = GetWaterSurfacePosition(position + new Vector3(-0.005f, 0f, 0f));

        Plane plane = new Plane(a, b, c);

        return plane.normal;
    }
}
