using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkullScoreManager : PlayerPanelController
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
}
