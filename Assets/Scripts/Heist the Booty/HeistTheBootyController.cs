using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private float rounds = 3;

    [SerializeField]
    private float pointsPerSecond = 1f;

    [SerializeField]
    private Animator overwatch;

    [SerializeField]
    private HeistTheBootyPlayerController[] players;

    [SerializeField, EventRef]
    private string coinPickupSound;

    public bool CanChicken { get; private set; } = false;


    private bool[] chicken = new bool[4];
    private bool isGivingPoints;
    private float currentTimer;



    private void Start()
    {
        StartCoroutine(Game());
    }

    private IEnumerator GiveScore()
    {
        while (isGivingPoints)
        {
            yield return new WaitForSeconds(1f / pointsPerSecond);
            GivePoints();
        }
    }

    private IEnumerator Game()
    {
        for (int i = 0; i < rounds; i++)
        {
            yield return StartCoroutine(Round());
        }

        // End game 'animation'
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            players[i].RunToStart();
        }

        yield return new WaitForSeconds(5f);

        GameManager.EndGameStatic();
    }

    private IEnumerator Round()
    {
        CanChicken = false;

        // Wait until everyone is ready
        bool loop = true;
        while (loop)
        {
            loop = false;
            for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
            {
                if (!players[i].IsReadyToLoot)
                {
                    loop = true;
                }
            }
            yield return null;
        }

        Debug.Log("Everyone's ready to loot!");

        // Start looting
        players.ForEach(p => p.Loot());

        yield return new WaitForSeconds(0.3f);

        // Reset chickening
        chicken = new bool[4];
        // Allow chickening
        CanChicken = true;

        // Generate random values
        float timeUntilLook = Random.Range(6f, 20f);
        float currentTime = timeUntilLook;
        float slightRumble = Random.Range(0.4f, 0.6f) * timeUntilLook;
        float heavyRumble = Random.Range(0.1f, 0.3f) * timeUntilLook;
        int currentRumblePhase = 0; // Handle which phase we are in

        // Start giving points
        isGivingPoints = true;
        StartCoroutine(GiveScore());

        // Handle the rumbles and countdown
        while (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            currentTimer = currentTime;

            if (currentRumblePhase == 0 && currentTime < slightRumble)
            {
                currentRumblePhase = 1;
                // Play rumble
                for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
                {
                    if (!chicken[i])
                    {
                        Vibrator.Instance.Vibrate(i, 1, 0.25f);
                    }
                }
            }
            else if (currentRumblePhase == 1 && currentTime < heavyRumble)
            {
                currentRumblePhase = 2;
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
        // End of round

        // Stop giving points
        isGivingPoints = false;

        // Stop controller vibrations
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            Vibrator.Instance.Vibrate(i, 0, 0f);
            Vibrator.Instance.Vibrate(i, 1, 0f);
        }

        CanChicken = false;

        // Shoot all players who haven't chickened
        yield return StartCoroutine(ShootPlayers());
 
        // Wait for a bit for the animations to settle
        yield return new WaitForSeconds(1.5f);

        // Move back to the looting position
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            players[i].MoveBackToLoot();
        }
    }

    int playerToShoot = 0;
    bool hasShotPlayer = false;
    private IEnumerator ShootPlayers()
    {
        overwatch.SetBool("IsAiming", true);

        // Wait until he is aimed, then a lil buffer
        yield return new WaitUntil(() => overwatch.GetCurrentAnimatorStateInfo(0).IsName("Aim Pistol"));
        yield return new WaitForSeconds(0.5f);

        // Shoot players who don't chicken out
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            if (chicken[i] == false)
            {
                playerToShoot = i;
                overwatch.SetTrigger("Shoot");
                yield return new WaitUntil(() => hasShotPlayer == true);
                hasShotPlayer = false;
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Go back to sleeping
        overwatch.SetBool("IsAiming", false);

        // Wait until he is sleeping, then a lil buffer
        yield return new WaitUntil(() => overwatch.GetCurrentAnimatorStateInfo(0).IsName("Sitting-LookingDown"));
        //yield return new WaitForSeconds(0.5f);
    }

    public void ShootPlayer()
    {
        if (!enabled) return; // If this isn't enabled, don't shoot them

        players[playerToShoot].GetShot();
        hasShotPlayer = true;
    }

    private void GivePoints()
    {
        bool playSound = false;
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            if (!chicken[i])
            {
                ScoreManager.Instance.AddScoreToPlayer(i, 1);
                playSound = true;
            }
        }
        if (playSound)
        {
            RuntimeManager.PlayOneShot(coinPickupSound);
        }
    }

    public bool ChickenOut(int playerNumber)
    {
        if (CanChicken && chicken[playerNumber] == false)
        {
            chicken[playerNumber] = true;
            Vibrator.Instance.Vibrate(playerNumber, 0, 0f);
            Vibrator.Instance.Vibrate(playerNumber, 1, 0f);
            return true;

        }
        else
        {
            return false;
        }
    }
}