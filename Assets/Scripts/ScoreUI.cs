using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public PlayerScore playerScore;

    private void OnEnable()
    {
        playerScore.onScoreUpdated += UpdateScore;
    }

    private void OnDisable()
    {
        playerScore.onScoreUpdated += UpdateScore;
    }

    private void UpdateScore()
    {
        scoreText.text = $"Score: {playerScore.Score}";
    }
}
