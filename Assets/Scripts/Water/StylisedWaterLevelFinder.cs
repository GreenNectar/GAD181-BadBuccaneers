using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylisedWaterLevelFinder : WaterLevelFinder
{
    [Tooltip("Need to use the shader given, will make this a reference later so we don't have to put it in")]
    public Shader shader;


    public override Vector3 GetWaterSurfacePosition(Vector3 position)
    {
        float waveMultiplier = 1f;
        float wave1Speed = 0.5f;
        float wave1Scale = 0.2f;
        float wave1Height = 3f;
        float wave1Rotation = 0f;
        float wave1Power = 1.25f;
        float time = _Time;//Shader.GetGlobalVector("_Time").y;//Time.deltaTime;

        float a = time * wave1Speed;
        float b = GetPosition(position, wave1Rotation) *  wave1Scale;

        float c = Mathf.Pow(1f - Mathf.Abs(Mathf.Sin(a + b)), wave1Power);

        float height = c * wave1Height;//Mathf.Lerp(0f, c * wave1Height, waveMultiplier);

        return new Vector3(position.x, height, position.z);// base.GetWaterSurfacePosition(position);
    }

    float GetPosition(Vector3 position, float rotation)
    {
        float x = Mathf.Sin(Mathf.Deg2Rad * rotation) * position.x;
        float z = Mathf.Cos(Mathf.Deg2Rad * rotation) * position.z;
        return x + z;
    }
}
