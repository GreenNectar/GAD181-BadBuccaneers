using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    //[SerializeField, Scene]
    //private string scoreScene;

    public static UnityEvent<int> onPlayerFinish = new UnityEvent<int>();

    public static UnityEvent<int, int> onPlayerScore = new UnityEvent<int, int>();

    public static UnityEvent onResultsFinish = new UnityEvent();

    public static UnityEvent<float> onTimerStart = new UnityEvent<float>();
    public static UnityEvent onTimerEnd = new UnityEvent();
    public static UnityEvent onTimerStop = new UnityEvent();
    public static UnityEvent onPlayerTimerStart = new UnityEvent();

    private static bool isTiming = false;

    public static void StartTimer(float time)
    {
        onTimerStart.Invoke(time);
        Instance.StartCoroutine(CountDownTimer(time));
    }

    public static void StopTimer()
    {
        onTimerStop.Invoke();
        if (isTiming)
        {
            Instance.StopAllCoroutines();
        }
    }

    private static IEnumerator CountDownTimer(float time)
    {
        isTiming = true;
        yield return new WaitForSeconds(time);
        isTiming = false;
        onTimerEnd.Invoke();
    }

    public static void AddScoreToPlayer(int player, int score)
    {
        onPlayerScore.Invoke(player, score);
        //FindObjectsOfType<ScorePlayerPanelController>().First(s => s.PlayerNumber == player).AddScore(score);
    }

    public static void PlayerFinish(int player)
    {
        onPlayerFinish.Invoke(player);
    }

    public static void FinishLevel(float time = 0f)
    {
        if (time == 0f)
        {
            FinishLevelMethod();
        }
        else
        {
            Instance.StartCoroutine(FinishLevelSequence(time));
        }
    }

    private static IEnumerator FinishLevelSequence(float time)
    {
        yield return new WaitForSeconds(time);
        FinishLevelMethod();
    }

    private static void FinishLevelMethod()
    {
        if (FindObjectOfType<ScorePlayerPanelController>())
        {
            int points = 3;
            int pointsToAdd = points;
            int previousScore = -1;
            foreach (var score in FindObjectsOfType<ScorePlayerPanelController>().OrderBy(s => s.score))
            {
                if (previousScore == -1 || previousScore != score.score)
                {
                    pointsToAdd = points;
                }

                ScoreManager.Instance.scores.First(s => s.player == score.PlayerNumber).score += pointsToAdd;

                previousScore = score.score;

                points--;
            }
        }

        if (FindObjectOfType<TimedPlayerPanel>())
        {
            int points = 3;
            int pointsToAdd = points;
            float previousScore = -1f;
            foreach (var score in FindObjectsOfType<TimedPlayerPanel>().OrderBy(s => -s.time))
            {
                if (previousScore == -1 || previousScore != score.time)
                {
                    pointsToAdd = points;
                }

                ScoreManager.Instance.scores.First(s => s.player == score.PlayerNumber).score += pointsToAdd;

                previousScore = score.time;

                points--;
            }
        }

        GameManager.Instance.LoadResultsScreen();
    }

    public static void FinishResults()
    {
        onResultsFinish.Invoke();
    }
}