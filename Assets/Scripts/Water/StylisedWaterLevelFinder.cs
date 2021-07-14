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
        Vector2 gradientSpeed = material.GetVector("_GradientSpeed");//1f;
        float gradientScale = material.GetFloat("_GradientScale");//1f;



        float wave1Speed = material.GetFloat("_Wave1Speed");// 0.5f;
        float wave1Scale = material.GetFloat("_Wave1Scale");// 0.05f;
        float wave1Height = material.GetFloat("_Wave1Height");// 6f;
        float wave1Rotation = material.GetFloat("_Wave1Rotation");// 0f;
        float wave1Power = material.GetFloat("_Wave1Power");// 1.25f;
        float time = _Time;//Shader.GetGlobalVector("_Time").y;//Time.deltaTime;

        float t = time * wave1Speed;
        float p = GetPosition(position, wave1Rotation) * wave1Scale;
        float wave1Final = WaterHeight(p, t, wave1Power);//Mathf.Pow(1f - Mathf.Abs(Mathf.Sin(a + b)), wave1Power);




        float wave2Speed = material.GetFloat("_Wave2Speed");// -1f;
        float wave2Scale = material.GetFloat("_Wave2Scale");// 0.1f;
        float wave2Height = material.GetFloat("_Wave2Height");// 1f;
        float wave2Rotation = material.GetFloat("_Wave2Rotation");// 277f;
        float wave2Power = material.GetFloat("_Wave2Power");// 1f;

        t = time * wave2Speed;
        p = GetPosition(position, wave2Rotation) * wave2Scale;
        float wave2Final = WaterHeight(p, t, wave2Power);




        float wave3Speed = material.GetFloat("_Wave3Speed");// 0.5f;
        float wave3Scale = material.GetFloat("_Wave3Scale");// 0.44f;
        float wave3Height = material.GetFloat("_Wave3Height");// 1f;
        float wave3Rotation = material.GetFloat("_Wave3Rotation");// 207f;
        float wave3Power = material.GetFloat("_Wave3Power");// 1f;

        t = time * wave3Speed;
        p = GetPosition(position, wave3Rotation) * wave3Scale;
        float wave3Final = WaterHeight(p, t, wave3Power);



        float height = ((wave1Final * wave1Height) + (wave2Final * wave2Height) + (wave3Final * wave3Height)) * waveMultiplier;//Mathf.Lerp(0f, c * wave1Height, waveMultiplier);

        return new Vector3(position.x, height, position.z);// base.GetWaterSurfacePosition(position);
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


    // The GRAVEYARD ;-;

    /*
    Vector2 TilingAndOffset(Vector2 uv, Vector2 Tiling, Vector2 Offset)
    {
        return uv * Tiling + Offset;
    }

    float GetWaterGradientNoise(Vector3 position, Vector2 speed, float scale)
    {
        Vector2 p = new Vector2(position.x, position.z);

        Vector2 offset = _Time * speed;

        return Mathf.PerlinNoise((position.x + offset.x) * scale, (position.y + offset.y) * scale);

        //return Noise(TilingAndOffset(p, new Vector2(1f, 1f), offset), scale);//gradientNoise(TilingAndOffset(p, new Vector2(1f, 1f), offset), scale);//UnityGradientNoise(TilingAndOffset(p, new Vector2(1f, 1f), offset), scale);
    }
    */

    /*
    #region SimpleNoise

    private float Frac(float value) { return value - (float)Math.Truncate(value); }

    private float RandomValue(Vector2 uv)
    {
        return Frac(Mathf.Sin(Vector2.Dot(uv, new Vector2(12.9898f, 78.233f))) * 43758.5453f);
    }

    private float Interpolate(float a, float b, float t)
    {
        return (1f - t) * a + (t * b);
    }

    private float NoiseValue(Vector2 uv)
    {
        Vector2 i = new Vector2(Mathf.Floor(uv.x), Mathf.Floor(uv.y));
        Vector2 f = new Vector2(Frac(uv.x), Frac(uv.y));

        f.x = f.x * f.x * (3f - 2f * f.x);
        f.y = f.y * f.y * (3f - 2f * f.y);

        uv.x = Mathf.Abs(Frac(uv.x) - 0.5f);
        uv.y = Mathf.Abs(Frac(uv.y) - 0.5f);

        Vector2 c0 = i + new Vector2(0, 0);
        Vector2 c1 = i + new Vector2(1, 0);
        Vector2 c2 = i + new Vector2(0, 1);
        Vector2 c3 = i + new Vector2(1, 1);

        float r0 = RandomValue(c0);
        float r1 = RandomValue(c1);
        float r2 = RandomValue(c2);
        float r3 = RandomValue(c3);

        float bottomOfGrid = Interpolate(r0, r1, f.x);
        float topOfGrid = Interpolate(r2, r3, f.x);
        return Interpolate(bottomOfGrid, topOfGrid, f.y);
    }

    private float Noise(Vector2 uv, float scale)
    {
        float t = 0f;

        float freq = Mathf.Pow(2f, 0f);
        float amp = Mathf.Pow(0.5f, 3f);
        t += NoiseValue(new Vector2(uv.x*scale/freq, uv.y*scale/freq))*amp;

        freq = Mathf.Pow(2f, 1f);
        amp = Mathf.Pow(0.5f, 2f);
        t += NoiseValue(new Vector2(uv.x * scale / freq, uv.y * scale / freq)) * amp;

        freq = Mathf.Pow(2f, 2f);
        amp = Mathf.Pow(0.5f, 1f);
        t += NoiseValue(new Vector2(uv.x * scale / freq, uv.y * scale / freq)) * amp;

        return t;
    }

    #endregion
    */

    //#region Gradient Noise

    //public float Frac(float value) { return value - (float)Math.Truncate(value); }

    //Vector2 UnityGradientNoiseDir(Vector2 p)
    //{
    //    p.x %= 289f;
    //    p.y %= 289f;

    //    float x = (float)(34f * p.x + 1f) * p.x % 289f + p.y;
    //    x = (34f * x + 1f) * x % 289f;
    //    x = ((x / 41f) % 1f) * 2f - 1f;//Frac(x / 41f) * 2f - 1f;
    //    return new Vector2(x - Mathf.Floor(x + 0.5f), Mathf.Abs(x) - 0.5f).normalized;
    //}

    //float UnityGradientNoise(Vector2 uv, float scale)
    //{
    //    uv *= scale;
    //    Vector2 ip = new Vector2(Mathf.Floor(uv.x), Mathf.Floor(uv.y));
    //    Vector2 fp = new Vector2(Frac(uv.x), Frac(uv.y));
    //    float d00 = Vector2.Dot(UnityGradientNoiseDir(ip), fp);
    //    float d01 = Vector2.Dot(UnityGradientNoiseDir(ip + new Vector2(0f, 1f)), fp - new Vector2(0f, 1f));
    //    float d10 = Vector2.Dot(UnityGradientNoiseDir(ip + new Vector2(1f, 0f)), fp - new Vector2(1f, 0f));
    //    float d11 = Vector2.Dot(UnityGradientNoiseDir(ip + new Vector2(1f, 1f)), fp - new Vector2(1f, 1f));

    //    //fp = fp * fp * fp * (fp * (fp * 6f - (Vector2.one * 15f)) + (Vector2.one * 0.5f));

    //    fp.x = fp.x * fp.x * fp.x * (fp.x * (fp.x * 6f - 15f) + 10f);
    //    fp.y = fp.y * fp.y * fp.y * (fp.y * (fp.y * 6f - 15f) + 10f);
    //    return Mathf.Lerp(Mathf.Lerp(d00, d01, fp.y), Mathf.Lerp(d10, d11, fp.y), fp.x) + 0.5f;
    //}

    //float NoiseSinWave(Vector2 position, Vector2 minMax)
    //{
    //    float sinIn = Mathf.Sin(position.x) + Mathf.Sin(position.y);
    //    float sinInOffset = Mathf.Sin(position.x + 1f) + Mathf.Sin(position.y + 1f);
    //    float randomNo = Frac(Mathf.Sin((sinIn - sinInOffset) * (12.9898f + 78.233f)) * 43758.5453f);
    //    float noise = Mathf.Lerp(minMax.x, minMax.y, randomNo);
    //    return sinIn + noise;
    //}

    //Vector2 gradientNoisedir(Vector2 p)
    //{
    //    p = new Vector2(p.x % 289, p.y % 289);
    //    float x = (34 * p.x + 1) * p.x % 289 + p.y;
    //    x = (34 * x + 1) * x % 289;
    //    x = ((x / 41) % 1) * 2 - 1;
    //    return (new Vector2(x - Mathf.Floor(x + 0.5f), Mathf.Abs(x) - 0.5f)).normalized;
    //}

    //float gradientNoise(Vector2 uv, float scale)
    //{
    //    uv *= scale;
    //    Vector2 ip = new Vector2(Mathf.Floor(uv.x), Mathf.Floor(uv.y));
    //    Vector2 fp = new Vector2(uv.x % 1, uv.y % 1);
    //    float d00 = Vector3.Dot(gradientNoisedir(ip), fp);
    //    float d01 = Vector3.Dot(gradientNoisedir(ip + new Vector2(0, 1)), fp - new Vector2(0, 1));
    //    float d10 = Vector3.Dot(gradientNoisedir(ip + new Vector2(1, 0)), fp - new Vector2(1, 0));
    //    float d11 = Vector3.Dot(gradientNoisedir(ip + new Vector2(1, 1)), fp - new Vector2(1, 1));
    //    fp = fp * fp * fp * (fp * (fp * 6 - new Vector2(15, 15)) + new Vector2(10, 10));
    //    return Mathf.Lerp(Mathf.Lerp(d00, d01, fp.y), Mathf.Lerp(d10, d11, fp.y), fp.x);
    //}

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

    //#endregion
}
