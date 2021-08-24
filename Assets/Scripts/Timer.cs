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

    [SerializeField]
    private bool startOnPlayerFinish = false;

    public UnityEvent onStart;
    public UnityEvent onFinish;
    public UnityEvent onStop;

    private bool isTiming;

    private void OnEnable()
    {
        if (startOnPlayerFinish)
        {
            EventManager.onPlayerFinish.AddListener((a) => StartTimer());
        }
    }

    private void OnDisable()
    {
        if (startOnPlayerFinish)
        {
            EventManager.onPlayerFinish.RemoveListener((a) => StartTimer());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!startOnPlayerFinish)
        {
            StartTimer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTiming)
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
    }

    public void ResetTimer()
    {
        currentTime = 0f;
    }

    private void StopTimer()
    {
        isTiming = false;
        onStop.Invoke();
    }

    private void StartTimer()
    {
        isTiming = true;
        timerText.gameObject.SetActive(true);
        timerText.transform.parent.gameObject.SetActive(true);
        onStart.Invoke();
    }
}
