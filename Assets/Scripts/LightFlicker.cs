using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private Light lightSource;

    [Range(0, 40), SerializeField]
    private float flickerSpeed = 10f;

    [Range(0, 100), SerializeField]
    private float flickerPercent = 100f;

    [SerializeField]
    private bool randomNoiseSeed = true;
    [SerializeField]
    private float noiseSeed = 0f;

    private float initialIntensity;
    private float noiseX = 0f;


    // Start is called before the first frame update
    void Start()
    {
        initialIntensity = lightSource.intensity;
        if (randomNoiseSeed) noiseX = Random.Range(0f, 1000f); else noiseX = noiseSeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!lightSource.enabled) return;

        float flicker = 1f - (flickerPercent / 100f); // Get the flicker amount
        float noise = Mathf.PerlinNoise(noiseX, 0f) + flicker; // Add noise to the flicker
        noise = Mathf.Clamp(noise, flicker, 1f); // Clamp the noise between the flicker and 1
        // I should have commented this before...
        if (1f - flicker != 0f)
            noise = (noise - 1f) / (1f - flicker) + 1f;
        else
            noise = 1f;

        lightSource.intensity = noise * initialIntensity; // Multiply the noise by the intensity
        noiseX += Time.deltaTime * flickerSpeed;
    }
}
