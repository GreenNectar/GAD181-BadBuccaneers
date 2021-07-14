using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylisedWaterLevelFinder : WaterLevelFinder
{
    //[Tooltip("Need to use the shader given, will make this a reference later so we don't have to put it in")]
    //public Shader shader;
    public Material material;

    //TODO 
    public override Vector3 GetWaterSurfacePosition(Vector3 position)
    {
        float waveMultiplier = material.GetFloat("_WaveMultiplier");//1f;
        float gradientSpeed = material.GetFloat("_GradientSpeed");//1f;
        float gradientScale = material.GetFloat("_GradientScale");//1f;



        float wave1Speed = material.GetFloat("_Wave1Speed");// 0.5f;
        float wave1Scale = material.GetFloat("_Wave1Scale");// 0.05f;
        float wave1Height = material.GetFloat("_Wave1Height");// 6f;
        float wave1Rotation = material.GetFloat("_Wave1Rotation");// 0f;
        float wave1Power = material.GetFloat("_Wave1Power");// 1.25f;
        float time = _Time;//Shader.GetGlobalVector("_Time").y;//Time.deltaTime;

        float a = time * wave1Speed;
        float b = GetPosition(position, wave1Rotation) * wave1Scale;
        float wave1Final = WaterHeight(b, a, wave1Power);//Mathf.Pow(1f - Mathf.Abs(Mathf.Sin(a + b)), wave1Power);




        float wave2Speed = material.GetFloat("_Wave2Speed");// -1f;
        float wave2Scale = material.GetFloat("_Wave2Scale");// 0.1f;
        float wave2Height = material.GetFloat("_Wave2Height");// 1f;
        float wave2Rotation = material.GetFloat("_Wave2Rotation");// 277f;
        float wave2Power = material.GetFloat("_Wave2Power");// 1f;

        a = time * wave2Speed;
        b = GetPosition(position, wave2Rotation) * wave2Scale;
        float wave2Final = WaterHeight(b, a, wave2Power);




        float wave3Speed = material.GetFloat("_Wave3Speed");// 0.5f;
        float wave3Scale = material.GetFloat("_Wave3Scale");// 0.44f;
        float wave3Height = material.GetFloat("_Wave3Height");// 1f;
        float wave3Rotation = material.GetFloat("_Wave3Rotation");// 207f;
        float wave3Power = material.GetFloat("_Wave3Power");// 1f;

        a = time * wave3Speed;
        b = GetPosition(position, wave3Rotation) * wave3Scale;
        float wave3Final = WaterHeight(b, a, wave3Power);



        float height = ((wave1Final * wave1Height) + (wave2Final * wave2Height) + (wave3Final * wave3Height)) * waveMultiplier;//Mathf.Lerp(0f, c * wave1Height, waveMultiplier);

        return new Vector3(position.x, height, position.z);// base.GetWaterSurfacePosition(position);
    }

    float GetGradient(Vector2 position, float speed, float scale)
    {
        return 0;
    }

    float GetPosition(Vector3 position, float rotation)
    {
        float x = Mathf.Sin(Mathf.Deg2Rad * rotation) * position.x;
        float z = Mathf.Cos(Mathf.Deg2Rad * rotation) * position.z;
        return x + z;
    }

    float WaterHeight(float position, float time, float power)
    {
        return Mathf.Pow(1f - Mathf.Abs(Mathf.Sin(position + time)), power);
    }

    #region Gradient Noise

    public float Frac(float value) { return value - (float)Math.Truncate(value); }

    Vector2 UnityGradientNoiseDir(Vector2 p)
    {
        p.x %= 289;
        p.y %= 289;

        float x = (34 * p.x + 1) * p.x % 289 + p.y;
        x = (34 * x + 1) * x % 289;
        x = Frac(x / 41) * 2 - 1;
        return new Vector2(x - Mathf.Floor(x + 0.5f), Mathf.Abs(x) - 0.5f).normalized;
    }

    float UnityGradientNoise(Vector2 p)
    {
        Vector2 ip = new Vector2(Mathf.Floor(p.x), Mathf.Floor(p.y));
        Vector2 fp = new Vector2(Frac(p.x), Frac(p.y));
        float d00 = Vector2.Dot(UnityGradientNoiseDir(ip), fp);
        float d01 = Vector2.Dot(UnityGradientNoiseDir(ip + new Vector2(0f, 1f)), fp - new Vector2(0f, 1f));
        float d10 = Vector2.Dot(UnityGradientNoiseDir(ip + new Vector2(1f, 0f)), fp - new Vector2(1f, 0f));
        float d11 = Vector2.Dot(UnityGradientNoiseDir(ip + new Vector2(1f, 1f)), fp - new Vector2(1f, 1f));

        fp.x = fp.x * fp.x * fp.x * (fp.x * (fp.x * 6f - 15f) + 10f);
        fp.y = fp.y * fp.y * fp.y * (fp.y * (fp.y * 6f - 15f) + 10f);
        return Mathf.Lerp(Mathf.Lerp(d00, d01, fp.y), Mathf.Lerp(d10, d11, fp.y), fp.x);
    }

    float UnityGradientNoiseFloat(Vector2 UV, float Scale)
    {
        return UnityGradientNoise(UV * Scale) + 0.5f;
    }

    //float2 unity_gradientNoise_dir(float2 p)
    //{
    //    p = p % 289;
    //    float x = (34 * p.x + 1) * p.x % 289 + p.y;
    //    x = (34 * x + 1) * x % 289;
    //    x = frac(x / 41) * 2 - 1;
    //    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
    //}

    //float unity_gradientNoise(float2 p)
    //{
    //    float2 ip = floor(p);
    //    float2 fp = frac(p);
    //    float d00 = dot(unity_gradientNoise_dir(ip), fp);
    //    float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
    //    float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
    //    float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
    //    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    //    return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
    //}

    //void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
    //{
    //    Out = unity_gradientNoise(UV * Scale) + 0.5;
    //}

    #endregion

    #region Tiling and Offset



    #endregion
}
