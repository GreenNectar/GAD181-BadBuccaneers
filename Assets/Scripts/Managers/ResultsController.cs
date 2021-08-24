using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsController : MonoBehaviour
{
    [Header("Main")]
    public int playerNumber;
    public GameObject player;
    [SerializeField]
    private Animator animator;
    [SerializeField, EventRef]
    private string coinEvent;

    [Header("Main Score")]
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private Image coinImage;
    [SerializeField]
    private RectTransform coins;

    [Header("Added Score")]
    [SerializeField]
    private RectTransform addedCoins;
    [SerializeField]
    private CanvasGroup addedCoinsCanvasGroup;
    [SerializeField]
    private TextMeshProUGUI addedCoinsScore;
    [SerializeField]
    private float addedCoinsOffset = 40f;

    [Header("Display Stuff")]
    [SerializeField]
    private Image positionChange;
    [SerializeField]
    private CanvasGroup positionChangeCanvasGroup;
    [SerializeField]
    private TextMeshProUGUI position;
    [SerializeField]
    private CanvasGroup positionCanvasGroup;


    public bool IsIncrementing { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        // If the player is not added to the manager, disable this gameobject
        if (playerNumber > PlayerManager.PlayerCount - 1 && PlayerManager.PlayerCount != 0)
        {
            player.SetActive(false);
            gameObject.SetActive(false);
        }

        addedCoinsCanvasGroup.alpha = 0f;
        positionChangeCanvasGroup.alpha = 0f;
        positionCanvasGroup.alpha = 0f;
    }

    public void UpdatePosition(int pos)
    {
        switch (pos)
        {
            case 1:
                position.text = "<color=yellow>1st</color>";
                break;
            //case 4:
            //    position.text = "<color=red>4th</color>";
            //    break;
            default:
                position.text = pos.Ordinal();
                break;
        }
    }

    public void UpdateScoreText(int score)
    {
        this.score.text = score.ToString();
    }

    public void StartIncrementScore(int from, int to)
    {
        IsIncrementing = true;
        StopAllCoroutines();
        StartCoroutine(IncrementScore(from, to));
    }

    private IEnumerator IncrementScore(int from, int to)
    {
        // Initialise some shit
        int coinDifference = Mathf.Abs(to - from);
        addedCoinsScore.text = "+" + coinDifference.ToString();

        // Show addition coins
        float time = 1f;
        while (time > 0f)
        {
            time -= Time.deltaTime * 3f;
            time = Mathf.Clamp(time, 0f, 1f);

            addedCoinsCanvasGroup.alpha = 1f - time;

            // Move the added coins
            Vector3 temp = addedCoins.position;
            temp.y = coins.position.y + ((1f - time) * addedCoinsOffset);
            addedCoins.position = temp;

            yield return null;
        }

        // Wait a bit
        yield return new WaitForSeconds(1f);

        // Increment counter, and bob coin
        int scoreNumber = from;
        Vector2 starting = coinImage.rectTransform.anchoredPosition;
        for (int i = 0; i < to - from; i++)
        {
            bool hasIncremented = false;
            time = 1f;
            while (time > 0f)
            {
                if (time > 0.5f && !hasIncremented)
                {
                    hasIncremented = true;
                    // Change main score
                    scoreNumber++;
                    UpdateScoreText(scoreNumber);
                    // Change addition score
                    coinDifference--;
                    addedCoinsScore.text = "+" + coinDifference.ToString();
                    // Play coin sound
                    RuntimeManager.PlayOneShot(coinEvent);
                }

                coinImage.rectTransform.anchoredPosition = starting + Vector2.up * Mathf.Sin(time) * 50f;
                time -= Time.deltaTime * 3f;
                yield return null;
            }

            coinImage.rectTransform.anchoredPosition = starting;
            yield return null;
        }

        // Wait a bit
        yield return new WaitForSeconds(0.5f);

        // Hide addition coins
        time = 1f;
        while (time > 0f)
        {
            time -= Time.deltaTime * 3f;
            time = Mathf.Clamp(time, 0f, 1f);

            addedCoinsCanvasGroup.alpha = time;

            // Move the added coins
            Vector3 temp = addedCoins.position;
            temp.y = coins.position.y + (time * addedCoinsOffset);
            addedCoins.position = temp;

            yield return null;
        }

        IsIncrementing = false;

        yield return null;
    }

    /// <summary>
    /// Shows the arrow progression
    /// </summary>
    /// <param name="show">Whether it's showing or hiding</param>
    /// <param name="isProgressing">Whether the arrow is up or down</param>
    public void ShowArrow(bool show, bool isProgressing)
    {
        // Only change arrow if we are showing not hiding
        if (show)
        {
            // Is red and pointing down when dropping down a position, and white and pointing up when progressing
            positionChange.color = isProgressing ? Color.white : Color.red;
            positionChange.transform.rotation = isProgressing ? Quaternion.identity : Quaternion.Euler(180f, 0f, 0f);
        }

        //positionChangeCanvasGroup.alpha = show ? 0f : 1f;

        StartCoroutine(ShowArrowSequence(show));
    }

    public IEnumerator ShowArrowSequence(bool show)
    {
        float time = 1f;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            time = Mathf.Clamp(time, 0f, 1f);
            positionChangeCanvasGroup.alpha = show ? 1f - time : time;
            yield return null;
        }

        yield return null;
    }

    public void PlayReaction(bool isHappy)
    {
        if (isHappy)
        {
            PlayerManager.GetPlayerFMODEvent(playerNumber).Happy(player);
        }
        else
        {
            PlayerManager.GetPlayerFMODEvent(playerNumber).Sad(player);
        }
        Vibrator.Instance.PulseVibration(playerNumber, 0, 0.5f);
        animator.SetTrigger(isHappy ? "Progress" : "TheOpposite");
    }

    public void ShowOrdinal()
    {
        StartCoroutine(ShowOrdinalSequence());
    }

    public IEnumerator ShowOrdinalSequence()
    {
        float time = 1f;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            time = Mathf.Clamp(time, 0f, 1f);
            positionCanvasGroup.alpha = 1f - time;
            yield return null;
        }

        yield return null;
    }
}
