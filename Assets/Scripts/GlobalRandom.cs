using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Refactor and remove pre-generated numbers

/// <summary>
/// This is a class to make sure every frame's random number is the exact same
/// </summary>
public static class GlobalRandom
{
    //private static float[] randomNumbers;
    //private static float[] RandomNumbers
    //{
    //    get
    //    {
    //        if (randomNumbers == null || randomNumbers.Length == 0)
    //        {
    //            SetRandomNumbers();
    //        }
    //        return randomNumbers;
    //    }
    //}

    //private static void SetRandomNumbers()
    //{
    //    randomNumbers = new float[1024];

    //    for (int i = 0; i < 1024; i++)
    //    {
    //        randomNumbers[i] = Random.Range(0f, 1f);
    //    }
    //}

    //public static float Range(float min, float max)
    //{
    //    return RandomNumbers[Time.frameCount % randomNumbers.Length].Remap(0f, 1f, min, max);//((Time.frameCount % 256f) / 256f).Remap(0f, 1f, min, max);
    //}

    private static int generatedFrame = -1;
    private static float generatedNumber = 0;

    private static float GetRandomValue()
    {
        if (Time.frameCount != generatedFrame)
        {
            generatedFrame = Time.frameCount;
            generatedNumber = Random.Range(0f, float.MaxValue);
            return generatedNumber;
        }
        else
        {
            return generatedNumber;
        }
    }

    public static float Range(float min, float max)
    {

        return GetRandomValue().Remap(0f, 1f, min, max);
    }
}
