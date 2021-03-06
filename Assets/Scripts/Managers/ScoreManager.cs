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
    public int[] playerPositions = new int[4];
    public float[] playerStartTime = new float[4];
    public float[] playerTime = new float[4];
    public int maximumPoints = 0; // Used for percentage
    public bool isGivingPoints = false;

    public Score[] oldScores { get; set; } = new Score[]{
        new Score { player = 0, position = 0 },
        new Score { player = 1, position = 1 },
        new Score { player = 2, position = 2 },
        new Score { player = 3, position = 3 }};
    public Score[] scores { get; set; } = new Score[]{
        new Score { player = 0, position = 0 },
        new Score { player = 1, position = 1 },
        new Score { player = 2, position = 2 },
        new Score { player = 3, position = 3 }};

    public bool AllPlayersFinished => playersEnded.Count >= PlayerManager.PlayerCountScaled;

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
            // Get the scores (some are negative as the scores are sorted from highest to lowest, so we want to do the opposite)
            Dictionary<int, float> roundScores = new Dictionary<int, float>();
            switch (GameManager.Instance.currentMicroGame.scoreType)
            {
                case MicroGame.ScoreType.Points:
                    // Connect the scores to the player
                    for (int i = 0; i < PlayerManager.PlayerCount; i++)
                    {
                        roundScores.Add(i, playerPoints[i]);
                    }
                    break;
                case MicroGame.ScoreType.Percentage:
                    // Connect the scores to the player
                    for (int i = 0; i < PlayerManager.PlayerCount; i++)
                    {
                        roundScores.Add(i, playerPoints[i]);
                    }
                    break;
                case MicroGame.ScoreType.Elimination:
                    // Give the positions to the player
                    for (int i = 0; i < PlayerManager.PlayerCount; i++)
                    {
                        roundScores.Add(i, -playerPositions[i]);
                    }
                    break;
                case MicroGame.ScoreType.Race:
                    // Connect the times to the player
                    for (int i = 0; i < PlayerManager.PlayerCount; i++)
                    {
                        roundScores.Add(i, -playerTime[i] * (HasPlayerEnded(i) ? 1f : 0f));
                    }
                    break;
                default:
                    break;
            }

            // Give the scores
            //var a = var s in roundScores.OrderBy(key => key.Value);

            //https://stackoverflow.com/questions/289/how-do-you-sort-a-dictionary-by-value
            //var sortedTimes = roundScores.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            var sortedScores = from roundScore in roundScores orderby roundScore.Value descending select roundScore;
            // Use the sorted times to give the scores
            int score = 3;
            int previousScore = 3;
            float? previousValue = null;
            foreach (var sortedScore in sortedScores)
            {
                // sortedScore.Value is the score
                if (sortedScore.Value != 0f)
                {
                    if (previousValue == null)
                    {
                        scores.First(s => s.player == sortedScore.Key).score += score;
                        previousScore = score;
                        previousValue = sortedScore.Value;
                    }
                    else
                    {
                        if (sortedScore.Value == previousValue)
                        {
                            scores.First(s => s.player == sortedScore.Key).score += previousScore;
                        }
                        else
                        {
                            scores.First(s => s.player == sortedScore.Key).score += score;
                            previousScore = score;
                            previousValue = sortedScore.Value;
                        }
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
        playersEnded = new List<int>();
        playerPoints = new int[4];
        playerStartTime = new float[4];
        playerTime = new float[4];
        maximumPoints = 0;
        isGivingPoints = false;
    }

    public void ResetEverything()
    {
        ResetScores();
        oldScores = new Score[]{
            new Score { player = 0, position = 0 },
            new Score { player = 1, position = 1 },
            new Score { player = 2, position = 2 },
            new Score { player = 3, position = 3 }};
        scores = new Score[]{
            new Score { player = 0, position = 0 },
            new Score { player = 1, position = 1 },
            new Score { player = 2, position = 2 },
            new Score { player = 3, position = 3 }};
    }

    public void OnMicroGameLoad()
    {
        ResetScores();
    }

    public void EndFinalPlayers()
    {
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            if (!playersEnded.Contains(i))
            {
                playerPositions[i] = 1;
                playersEnded.Add(i);
                playerTime[i] = GlobalTimer.Time;

                if (playerTime[i] == 0f)
                {
                    playerTime[i] = GlobalTimer.Time;
                }
                EventManager.onPlayerFinish.Invoke();
            }
        }
    }

    public void EndPlayer(int player)
    {
        if (playersEnded.Contains(player)) throw new Exception("Player has already ended, please fix this!");

        playerPositions[player] = PlayerManager.PlayerCountScaled - playersEnded.Count;
        playersEnded.Add(player);
        playerTime[player] = GlobalTimer.Time;
        EventManager.onPlayerFinish.Invoke();
    }

    public bool HasPlayerEnded(int player)
    {
        return playersEnded.Contains(player);
    }

    public int GetScore(int player)
    {
        return playerPoints[player];
    }

    public void AddScoreToPlayer(int player, int score)
    {
        playerPoints[player] += score;
        EventManager.onUpdateScore.Invoke();
    }

    public void SetPlayerScore(int player, int score)
    {
        playerPoints[player] = score;
        EventManager.onUpdateScore.Invoke();
    }

    public void SetMaximumPoints(int score)
    {
        maximumPoints = score;
        EventManager.onUpdateScore.Invoke();
    }

    public void PlayScoreSoundEffect()
    {
        FMODUnity.RuntimeManager.PlayOneShot(SFXTable.Get("CoinDrop"));
    }
}