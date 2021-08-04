using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI position;
    [SerializeField]
    private TextMeshProUGUI wins;

    [SerializeField]
    public int playerNumber;

    public bool IsIncrementing { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        // If the player is not added to the manager, disable this gameobject (we don't want to render anything unnecessary)
        if (playerNumber > PlayerManager.PlayerCount - 1 && PlayerManager.PlayerCount != 0)
        {
            gameObject.SetActive(false);
        }

        playerName.text = $"Player {playerNumber + 1}";
    }


    //public void UpdateUI()
    //{
    //    playerName.text = $"Player {playerNumber + 1}";
    //    position.text = ScoreManager.Instance.GetPosition(playerNumber).Ordinal();
    //    UpdateWinText(ScoreManager.Instance.GetWins(playerNumber));
    //}

    public Vector2 GetPositionSize()
    {
        return position.rectTransform.localScale;
    }

    public void SetPositionSize(Vector2 scale)
    {
        position.rectTransform.localScale = scale;
    }

    public void UpdatePosition(int pos)
    {
        switch (pos)
        {
            case 1:
                position.text = "<color=yellow>1st</color>";
                break;
            case 4:
                position.text = "<color=red>4th</color>";
                break;
            default:
                position.text = pos.Ordinal();
                break;
        }
    }

    private void UpdateWinText(int number)
    {
        wins.text = "Score: " + number.ToString();
    }

    public void StartIncrementScore(int from, int to)
    {
        IsIncrementing = true;
        StopAllCoroutines();
        StartCoroutine(IncrementScore(from, to));
    }

    private IEnumerator IncrementScore(int from, int to)
    {
        int winsNumber = from;
        Vector2 starting = wins.rectTransform.anchoredPosition;
        for (int i = 0; i < to - from; i++)
        {
            bool hasIncremented = false;
            float time = 1f;
            while (time > 0f)
            {
                if (time > 0.5f && !hasIncremented)
                {
                    hasIncremented = true;
                    winsNumber++;
                    UpdateWinText(winsNumber);
                }

                wins.rectTransform.anchoredPosition = starting + Vector2.up * Mathf.Sin(time) * 50f;
                time -= Time.deltaTime * 5f;
                yield return null;
            }

            wins.rectTransform.anchoredPosition = starting;
            yield return null;
        }

        IsIncrementing = false;

        yield return null;
    }
}
