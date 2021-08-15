using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>, IMicroGameLoad
{
    // If the scoretype is elimination it will determine the losers, if it's race it'll determine the winners
    public List<int> playersEnded = new List<int>();
    public int[] playerPoints = new int[4];
    public float[] playerStartTime = new float[4];
    public float[] playerTime = new float[4];
    public int maximumPoints = 0; // Used for percentage

    public Score[] oldScores { get; set; } = new Score[]{
        new Score { player = 0 },
        new Score { player = 1 },
        new Score { player = 2 },
        new Score { player = 3 }};
    public Score[] scores { get; set; } = new Score[]{
        new Score { player = 0 },
        new Score { player = 1 },
        new Score { player = 2 },
        new Score { player = 3 }};

    [Serializable]
    public class Score
    {
        public int player;
        public int score;
        public int position;
    }

    public void StartTimer()
    {
        GlobalTimer.StartTimer();
    }



    public void FinaliseScores()
    {
        //TODO Give points to players from the level outcome

        if (GameManager.Instance.currentMicroGame)
        {
            // Get the scores
            Dictionary<int, float> roundScores = new Dictionary<int, float>();
            int score = 3;
            int previousScore = 3;
            switch (GameManager.Instance.currentMicroGame.scoreType)
            {
                case MicroGame.ScoreType.Points:
                    // Connect the times to the player
                    for (int i = 0; i < PlayerManager.PlayerCount; i++)
                    {
                        roundScores.Add(i, playerPoints[i]);
                    }
                    break;
                case MicroGame.ScoreType.Percentage:
                    break;
                case MicroGame.ScoreType.Elimination:
                    break;
                case MicroGame.ScoreType.Race:
                    // Connect the times to the player
                    for (int i = 0; i < PlayerManager.PlayerCount; i++)
                    {
                        roundScores.Add(i, -playerTime[i]);
                    }
                    break;
                default:
                    break;
            }

            // Give the scores
            //var a = var s in roundScores.OrderBy(key => key.Value);

            //https://stackoverflow.com/questions/289/how-do-you-sort-a-dictionary-by-value
            //var sortedTimes = roundScores.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            var sortedTimes = from time in roundScores orderby time.Value ascending select time;
            // Use the sorted times to give the scores
            float previous = -1f;
            foreach (var time in sortedTimes)
            {
                // 
                if (previous == -1f)
                {
                    scores[time.Key].score += score;
                    previousScore = score;
                    previous = time.Value;
                }
                else if (time.Value == 0f)
                {
                    scores[time.Key].score = 0;
                }
                else
                {
                    if (time.Value == previous)
                    {
                        scores[time.Key].score += previousScore;
                    }
                    else
                    {
                        scores[time.Key].score += score;
                        previousScore = score;
                        previous = time.Value;
                    }
                }

                score--;
            }
        }

        ResetScores();
    }

    public void ResetScores()
    {
        // Reset the values
        GlobalTimer.StopTimer();
        GlobalTimer.ResetTimer();
        playersEnded.Clear();
        playerPoints = new int[4];
        playerStartTime = new float[4];
        playerTime = new float[4];
        maximumPoints = 0;
    }

    public void OnMicroGameLoad()
    {
        ResetScores();
    }

    public void EndPlayer(int player)
    {
        playersEnded.Add(player);
        playerTime[player] = GlobalTimer.Time;
        EventManager.onPlayerFinish.Invoke(player);
    }

    public void AddScoreToPlayer(int player, int score)
    {
        playerPoints[player] += score;
        EventManager.onUpdateScore.Invoke(player);
    }

}