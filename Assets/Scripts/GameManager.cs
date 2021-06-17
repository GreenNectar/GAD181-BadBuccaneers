using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void ResetScores()
    {
        foreach (var score in FindObjectsOfType<PlayerScore>())
        {
            score.Clear();
        } 
    }
}
