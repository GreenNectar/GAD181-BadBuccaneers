using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    //public int[] wins { get; private set; } = new int[4];
    //public int[] position { get; private set; } = new int[4];
    public Score[] oldScores { get; set; } = new Score[]{
        new Score { player = 0, score = 0 },
        new Score { player = 1, score = 0 },
        new Score { player = 2, score = 0 },
        new Score { player = 3, score = 0 }};
    public Score[] scores { get; set; } = new Score[]{
        new Score { player = 0, score = 0 },
        new Score { player = 1, score = 0 },
        new Score { player = 2, score = 0 },
        new Score { player = 3, score = 0 }};
    //public int previousWinner = 0;

    [Serializable]
    public class Score
    {
        public int player;
        public int score;
        public int position;
    }
}
