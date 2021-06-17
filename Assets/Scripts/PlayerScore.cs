using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private int score;

    public int Score => score;

    public Action onScoreUpdated;

    public void AddScore(int score)
    {
        this.score += score;

        onScoreUpdated.Invoke();
    }

    public void Clear()
    {
        score = 0;

        onScoreUpdated.Invoke();
    }
}
