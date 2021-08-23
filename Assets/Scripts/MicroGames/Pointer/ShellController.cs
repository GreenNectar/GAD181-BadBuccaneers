using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShellController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Shell[] shells;

    [SerializeField]
    private GameObject pearl;

    [SerializeField]
    private PointerController[] players;

    [SerializeField]
    private Animator host;

    [Header("Main Values")]
    [SerializeField, Tooltip("How high off the table will the cups be")]
    private float height = 0.4f;

    [SerializeField, Tooltip("How long the wait time is for selecting the cup")]
    private float roundLength = 5f;

    [SerializeField]
    private ShellRound[] rounds;

    // Audio
    [SerializeField, EventRef]
    private string moveCupEvent;
    [SerializeField, ParamRef]
    private string roundParameter;
    private EventInstance[] cupMoveInstances;
    private int maxMoveInstances = 3;
    private int currMoveInstance = 0;


    private int currentRound = -1;
    private int playersVoted = 0;
    private int currentScore;
    private int[] scoresToGive;

    [Serializable]
    private class ShellRound
    {
        public int switches = 3;
        public float switchSpeed = 4f;
    }

    private bool HasAllVoted
    {
        get
        {
            return PlayerManager.PlayerCount > 0 && playersVoted == PlayerManager.PlayerCount;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Deactivate pearl
        pearl.SetActive(false);

        //votes = new int[players.Length];
        scoresToGive = new int[players.Length];
        ResetVotes();
        //EventManager.StartTimer(roundLength);
        StartCoroutine(RoundSequence());

        cupMoveInstances = new EventInstance[maxMoveInstances];
        for (int i = 0; i < cupMoveInstances.Length; i++)
        {
            cupMoveInstances[i] = RuntimeManager.CreateInstance(moveCupEvent);
        }
    }

    private void OnDisable()
    {
        // Clear up the memory babby
        for (int i = 0; i < maxMoveInstances; i++)
        {
            cupMoveInstances[i].release();
        }
    }

    public void Pressed(Shell shell, PointerController pointer)
    {
        //StartCoroutine(ShowCups());
        //for (int i = 0; i < shells.Length; i++)
        //{
        //    if (shells[i] == shell)
        //    {
        //        for (int j = 0; j < players.Length; j++)
        //        {
        //            if (players[j] == pointer)
        //            {
        //                votes[j] = i;
        //            }
        //        }
        //    }
        //}

        if (shell == shells[0])
        {
            for (int j = 0; j < players.Length; j++)
            {
                if (players[j] == pointer)
                {
                    scoresToGive[j] = currentScore--;
                }
            }
            //Debug.Log("Selected correct chalice");
        }
        else
        {
            for (int j = 0; j < players.Length; j++)
            {
                if (players[j] == pointer)
                {
                    scoresToGive[j] = 0;
                }
            }
            //Debug.Log("Selected wrong chalice");
        }

        playersVoted++;

        // Stop the player from voting
        pointer.gameObject.SetActive(false);
    }

    private void AllowVoting()
    {
        foreach (var player in players)
        {
            if (PlayerManager.HasPlayer(player.PlayerNumber) || PlayerManager.PlayerCount == 0)
            {
                player.gameObject.SetActive(true);
            }
        }
    }

    private void DisallowVoting()
    {
        foreach (var player in players)
        {
            player.gameObject.SetActive(false);
        }
    }

    private void ResetVotes()
    {
        //for (int i = 0; i < votes.Length; i++) votes[i] = -1;
        for (int i = 0; i < scoresToGive.Length; i++) scoresToGive[i] = -1;
        playersVoted = 0;
        currentScore = 3;
    }

    private IEnumerator RoundSequence()
    {
        // Set the round to the percentage completed
        RuntimeManager.StudioSystem.setParameterByName(roundParameter, ((float)currentRound / (float)rounds.Length));// - (1f / rounds.Length));

        // Initially show the cups on the first round
        if (currentRound == -1)
        {
            yield return new WaitForSeconds(1f);
            DisallowVoting();
            yield return StartCoroutine(ShowCups());
            currentRound++;
        }

        yield return new WaitForSeconds(1.5f);

        // Mix the shells
        yield return StartCoroutine(MixShells());

        yield return new WaitForSeconds(0.5f);

        // Start the timer
        TimeManager.StartTimer(roundLength);

        // Wait until the timer has finished, or until everyone has voted
        bool hasTimerFinished = false;
        EventManager.onTimerEnd.AddListener(() => hasTimerFinished = true);
        ResetVotes();
        AllowVoting();
        yield return new WaitUntil(() => hasTimerFinished || HasAllVoted);
        
        DisallowVoting();
        EventManager.onTimerEnd.RemoveListener(() => hasTimerFinished = true);
        // Stop the timer
        TimeManager.StopTimer();

        yield return new WaitForSeconds(0.5f);



        // Show the cups
        yield return StartCoroutine(ShowCups());

        // Go to next round
        currentRound++;
        if (currentRound < rounds.Length)
        {
            StartCoroutine(RoundSequence());
        }
        // Finish the game
        else
        {
            yield return new WaitForSeconds(2f);
            GameManager.Instance.EndGame();
        }

        yield return null;
    }

    private IEnumerator ShowCups()
    {
        float time = 0f;

        // Get the starting positions
        Vector3[] startingPositions = new Vector3[shells.Length];
        for (int i = 0; i < shells.Length; i++)
        {
            startingPositions[i] = shells[i].transform.localPosition;
        }

        // Set the pearls position
        Vector3 temp = shells[0].transform.position;
        temp.y = pearl.transform.position.y;
        pearl.transform.position = temp;
        pearl.SetActive(true);

        // Lift up
        while (time < 1f)
        {
            time = Mathf.Clamp(time + Time.deltaTime, 0f, 1f);

            for (int i = 0; i < shells.Length; i++)
            {
                Vector3 pos = startingPositions[i];
                pos.y += Mathfx.Berp(0f, height, time);
                shells[i].transform.localPosition = pos;
            }

            yield return null;
        }

        // Probably not a great place to put the scoring
        if (currentRound > -1)
        {
            // Give out points
            bool hasVotedCorrectly = false;
            for (int i = 0; i < players.Length; i++)
            {
                if (PlayerManager.PlayerCount > 0 && !PlayerManager.HasPlayer(i)) continue;

                if (scoresToGive[i] > 0)
                {
                    ScoreManager.Instance.AddScoreToPlayer(i, scoresToGive[i]);
                    hasVotedCorrectly = true;
                }
            }
            host.SetTrigger(hasVotedCorrectly ? "Correct" : "Incorrect");
        }

        yield return new WaitForSeconds(2f);

        // Put back down
        while (time > 0f)
        {
            time = Mathf.Clamp(time - Time.deltaTime, 0f, 1f);

            for (int i = 0; i < shells.Length; i++)
            {
                Vector3 pos = startingPositions[i];
                pos.y += Mathfx.Sinerp(height, 0f, 1f - time);
                shells[i].transform.localPosition = pos;
            }

            yield return null;
        }

        // Deactivate pearl
        pearl.SetActive(false);


        yield return null;
    }

    private IEnumerator MixShells()
    {
        if (shells.Length <= 1)
        {
            throw new Exception("Can't shuffle just one or no cups");
        }

        for (int i = 0; i < rounds[currentRound].switches; i++)
        {
            // Get the shells that we will switch
            int from = Random.Range(0, shells.Length);
            int to = from;
            while(to == from)
            {
                to = Random.Range(0, shells.Length);
            }

            // Store their starting positions
            Vector3 fromPosition = shells[from].transform.position;
            Vector3 toPosition = shells[to].transform.position;

            // Play the sfx
            cupMoveInstances[currMoveInstance].start();

            // Move each cup to the others position
            float time = 0f;
            while (time < 1f)
            {
                time = Mathf.Clamp(time + Time.deltaTime * rounds[currentRound].switchSpeed, 0f, 1f);
                // Sin time for the forward and backward movement
                float sinTime = Mathf.Sin(time * Mathf.PI);
                // We use Vector3 forward and back to make the cups not intersect
                shells[from].transform.position = Mathfx.Hermite(fromPosition, toPosition, time) + Vector3.forward * Mathfx.Hermite(0f, 0.25f, sinTime);
                shells[to].transform.position = Mathfx.Hermite(toPosition, fromPosition, time) + Vector3.back * Mathfx.Hermite(0f, 0.25f, sinTime);

                yield return null;
            }

            cupMoveInstances[currMoveInstance].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currMoveInstance = (currMoveInstance + 1) % maxMoveInstances;
        }
    }
}