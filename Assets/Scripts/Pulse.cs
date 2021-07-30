using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    [Tooltip("The frequency of the pulses per second")]
    public float pulseSpeed = 4f;

    [Tooltip("The scale offset it will scale to")]
    public float pulseScale = 1.5f;

    [Range(0, 1f)]
    public float pulseCutoff = 0f;

    private float time;
    public float AbsoluteSineTime
    {
        get
        {
            return (Mathf.Sin(Mathf.Deg2Rad * time) + 1f) / 2f;
        }
    }
    
    Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        time += 360f * pulseSpeed * Time.deltaTime;
        time %= 360f; // Gotta keep that precision baby

        float offset = (AbsoluteSineTime - pulseCutoff) / (1f - pulseCutoff);

        transform.localScale = originalScale * Mathf.Lerp(1f, pulseScale, offset);
    }

    public void ResetScale()
    {
        transform.localScale = originalScale;
    }
}
