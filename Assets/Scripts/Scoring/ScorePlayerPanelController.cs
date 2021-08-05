using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePlayerPanelController : PlayerPanelController
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    public int score { get; private set; }

    private void OnEnable()
    {
        EventManager.onPlayerScore.AddListener(AddScore);
    }

    private void OnDisable()
    {
        EventManager.onPlayerScore.AddListener(AddScore);
    }

    public void AddScore(int player, int score)
    {
        if (player != PlayerNumber) return;

        this.score += score;
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();
    }

    /*
    [SerializeField]
    private int score;

    public int Score => score;

    public Action onScoreUpdated;

    public void AddScore(int score)
    {
        this.score += score;

        onScoreUpdated.Invoke();
    }
    */

    //private void OnEnable()
    //{
    //    playerScore.onScoreUpdated += UpdateScore;
    //}

    //private void OnDisable()
    //{
    //    playerScore.onScoreUpdated += UpdateScore;
    //}

    //private void UpdateScore()
    //{
    //    scoreText.text = $"Score: {playerScore.Score}";
    //}
}
