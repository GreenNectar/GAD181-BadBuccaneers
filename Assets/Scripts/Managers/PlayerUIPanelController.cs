using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerUIPanelController : MonoBehaviour, IMicroGameLoad
{
    [SerializeField]
    private TextMeshProUGUI output;

    [SerializeField]
    private int playerNumber = 0;

    private void OnEnable()
    {
        EventManager.onUpdateScore.AddListener(SetPoints);
        EventManager.onPlayerFinish.AddListener(SetElimination);
    }

    private void OnDisable()
    {
        EventManager.onUpdateScore.RemoveListener(SetPoints);
        EventManager.onPlayerFinish.AddListener(SetElimination);
    }

    private void Update()
    {
        if (GlobalTimer.hasStarted)
        {
            SetTimed();
        }
    }

    private void SetPoints(int player)
    {
        if (player != playerNumber) return;

        output.text = ScoreManager.Instance.playerPoints[playerNumber].ToString();
    }

    private void SetPercentage(int player)
    {
        if (player != playerNumber) return;

        int playerPoints = ScoreManager.Instance.playerPoints[playerNumber];
        int maximumPoints = ScoreManager.Instance.maximumPoints;
        output.text = ((float)playerPoints / (float)maximumPoints).ToString();
    }

    private void SetElimination(int player)
    {
        if (player != playerNumber) return;

        output.text = ScoreManager.Instance.playerPositions[playerNumber].Ordinal();
    }

    private void SetFirstToEnd(int player)
    {
        if (player != playerNumber) return;

        int position = 1;

        foreach (int p in ScoreManager.Instance.playersEnded)
        {
            if (p == player) break;
            position++;
        }

        output.text = position.Ordinal();
    }

    private void SetTimed()
    {
        float time = ScoreManager.Instance.playersEnded.Contains(playerNumber) ? ScoreManager.Instance.playerTime[playerNumber] : GlobalTimer.Time;
        SetTime(time);
    }

    private void SetTime(float time)
    {
        int milliseconds = (int)((time % 1f) * 1000f);
        int seconds = (int)(time % 60f);
        int minutes = (int)(time / 60f);

        output.text = $"{minutes}:{seconds.ToString("D2")}:{milliseconds.ToString("D3").Substring(0, 2)}";
    }

    public void OnMicroGameLoad()
    {
        if (GameManager.Instance.currentMicroGame == null) return;

        switch (GameManager.Instance.currentMicroGame.scoreType)
        {
            case MicroGame.ScoreType.Points:
                output.text = "0";
                break;
            case MicroGame.ScoreType.Percentage:
                output.text = "0%";
                break;
            case MicroGame.ScoreType.Elimination:
                output.text = "";
                break;
            case MicroGame.ScoreType.Race:
                SetTime(0f);
                break;
            default:
                break;
        }
    }
}
