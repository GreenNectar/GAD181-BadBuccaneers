using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Timer is randomly set by game as 1 of several values
/// Players gain coins the longer they stay in the game
/// Eventually players will start to feel a slight rumble between 40-60% of remaining time
/// At 5-30% of remaining time heavy rumble
/// Button saves points but can't gain more
/// If you have not pressed button before time is up, lose all your points
/// </summary>
public class HeistTheBootyController : MonoBehaviour
{
    public float pointsPerSec = 1f;

    public bool[] chicken = new bool[4];
    private bool isGivingPoints;

    public float currentTimer;

    private void Start()
    {
        StartCoroutine(Round());
    }

    // Update is called once per frame
    void Update()
    {
        //if (chicken = false)
        //{
        //    scoreText.text = (int)score + "Score";
        //    score += pointsPerSec * Time.deltaTime;
        //}
    }

    private IEnumerator GiveScore()
    {
        while (isGivingPoints)
        {
            yield return new WaitForSeconds(1f / pointsPerSec);
            GivePoints();
        }
    }

    private IEnumerator Round()
    {
        float timeUntilLook = Random.Range(6f, 20f);
        float currentTime = timeUntilLook;
        float slightRumble = Random.Range(0.4f, 0.6f) * timeUntilLook;
        float heavyRumble = Random.Range(0.1f, 0.3f) * timeUntilLook;
        int currentRumble = 0;

        isGivingPoints = true;
        StartCoroutine(GiveScore());

        while (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            currentTimer = currentTime;

            if (currentRumble == 0 && currentTime < slightRumble)
            {
                currentRumble = 1;
                // Play rumble
                for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
                {
                    if (!chicken[i])
                    {
                        Vibrator.Instance.Vibrate(i, 1, 0.25f);
                    }
                }
            }
            else if (currentRumble == 1 && currentTime < heavyRumble)
            {
                currentRumble = 2;
                // Play large rumble
                for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
                {
                    if (!chicken[i])
                    {
                        Vibrator.Instance.Vibrate(i, 0, 1f);
                        Vibrator.Instance.Vibrate(i, 1, 1f);
                    }
                }
            }
            yield return null;
        }

        isGivingPoints = false;

        // End of round
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            Vibrator.Instance.Vibrate(i, 0, 0f);
            Vibrator.Instance.Vibrate(i, 1, 0f);
        }
    }

    private void GivePoints()
    {
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            if (!chicken[i])
            {
                ScoreManager.Instance.AddScoreToPlayer(i, 1);
            }
        }
    }

    public void ChickenOut(int playerNumber)
    {
        chicken[playerNumber] = true;
        Vibrator.Instance.Vibrate(playerNumber, 0, 0f);
        Vibrator.Instance.Vibrate(playerNumber, 1, 0f);
    }
}