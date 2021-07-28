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

    public UnityEvent<int> onPlayerWin = new UnityEvent<int>();

    public static UnityEvent<int, int> onPlayerScore = new UnityEvent<int, int>();

    public static UnityEvent onResultsFinish = new UnityEvent();

    public static void AddScoreToPlayer(int player, int score)
    {
        onPlayerScore.Invoke(player, score);
        //FindObjectsOfType<ScorePlayerPanelController>().First(s => s.PlayerNumber == player).AddScore(score);
    }

    public static void FinishLevel()
    {
        if (FindObjectOfType<ScorePlayerPanelController>())
        {
            int points = 3;
            int pointsToAdd = points;
            int previousScore = -1;
            foreach (var score in FindObjectsOfType<ScorePlayerPanelController>())
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

        GameManager.Instance.LoadResultsScreen();
    }

    public static void FinishResults()
    {
        onResultsFinish.Invoke();
    }
}