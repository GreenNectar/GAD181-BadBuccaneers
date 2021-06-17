using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private float maxTime = 30f;

    private float currentTime = 0f;

    public UnityEvent onStart;
    public UnityEvent onFinish;
    public UnityEvent onStop;

    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            onFinish.Invoke();
        }

        currentTime = Mathf.Clamp(currentTime, 0f, maxTime);

        timerText.text = $"{Mathf.Ceil(maxTime - currentTime)}";
    }

    public void ResetTimer()
    {
        currentTime = 0f;
    }

    private void StopTimer()
    {
        onStop.Invoke();
    }

    private void StartTimer()
    {
        onStart.Invoke();
    }
}
