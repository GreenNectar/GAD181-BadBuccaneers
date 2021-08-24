using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static ScoreManager;

public class ResultsManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions;

    [SerializeField]
    private Transform[] players;

    [SerializeField]
    private ResultsController[] resultsControllers;

    int[] oldScores = new int[4];
    int[] newScores = new int[4];

    bool isFirstResult = true;

    private Score[] OldScores
    {
        get
        {
            return ScoreManager.Instance.oldScores;
        }
        set
        {
            ScoreManager.Instance.oldScores = value;
        }
    }
    private Score[] Scores
    {
        get
        {
            return ScoreManager.Instance.scores;
        }
        set
        {
            ScoreManager.Instance.scores = value;
        }
    }

    public int GetScore(int player, Score[] scores)
    {
        return scores.First(s => s.player == player).score;
    }

    //private void Start()
    //{
    //    Invoke("ResultsSequence", 2f);
    //}

    private void Start()
    {
        Initialise();
        SortScores();
        Invoke("ResultsSequence", 2f);
        //ResultsSequence();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Scores.First(s => s.player == 0).score++;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Scores.First(s => s.player == 1).score++;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Scores.First(s => s.player == 2).score++;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Scores.First(s => s.player == 3).score++;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SortScores();
            ResultsSequence();
        }
    }
#endif

    private void Initialise()
    {
        foreach (var result in FindObjectsOfType<ResultsController>())
        {
            result.UpdateScoreText(OldScores[result.playerNumber].score);
        }

        SortScores();

        // Put them on their correct location
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            players[OldScores[i].player].position = positions[i].position;
        }
    }

    private void SortScores()
    {
        // Sort the scores
        Scores = Scores.OrderBy(s => -s.score).ToArray();
        OldScores = OldScores.OrderBy(s => -s.score).ToArray();

        // Set the ordinal positions
        SetScorePositions(Scores);
        SetScorePositions(OldScores);

        // Get the scores ordered by player
        oldScores = OldScores.OrderBy(s => s.player).Select(s => s.score).ToArray();
        newScores = Scores.OrderBy(s => s.player).Select(s => s.score).ToArray();
    }

    private void ResultsSequence()
    {
        // Increment the counters
        foreach (var resultController in resultsControllers.OrderBy(r => r.playerNumber))
        {
            resultController.UpdatePosition(OldScores.First(s => s.player == resultController.playerNumber).position);

            int p = resultController.playerNumber;
            int from = oldScores[p];
            int to = newScores[p];

            resultController.StartIncrementScore(from, to);
        }

        // Start the sort animation
        StartCoroutine(ScoreSequence());

        // Copy the new score score into the old score
        foreach (var score in Scores)
        {
            foreach (var oldScore in OldScores)
            {
                if (oldScore.player == score.player)
                {
                    oldScore.score = score.score;
                }
            }
        }
    }

    private void SetScorePositions(Score[] scores)
    {
        // Just makes it go 1st 2nd 2nd 4th (gives the same ordinal position for players with the same score)
        for (int i = 0; i < scores.Length; i++)
        {
            if (i > 0 && scores[i].score == scores[i - 1].score)
            {
                scores[i].position = scores[i - 1].position;
            }
            else
            {
                scores[i].position = i + 1;
            }

            //scores[i].position = i + 1;
        }
    }

    private IEnumerator ScoreSequence()
    {
        // We want to wait for the values to set in
        yield return new WaitForEndOfFrame();

        // We want to wait until they have stopped their incrementing animation
        yield return new WaitUntil(() =>
        {
            foreach (var result in resultsControllers)
            {
                if (result.isActiveAndEnabled && result.IsIncrementing)
                {
                    return false;
                }
            }

            return true;
        });

        // Animate ordinals disappearing
        //float time = 0f;
        //while (time < 1f)
        //{
        //    time += Time.deltaTime * 4f;
        //    for (int i = 0; i < resultsControllers.Length; i++)
        //    {
        //        resultsControllers[i].SetPositionSize(Vector2.Lerp(scales[i], Vector2.zero, time));
        //    }
        //    yield return null;
        //}

        // Show progress
        string[] progress = new string[PlayerManager.PlayerCountScaled];
        bool hasChangedPositions = false;
        for (int i = 0; i < progress.Length; i++)
        {
            int previousPosition = OldScores.First(s => s.player == i).position;
            int newPosition = Scores.First(s => s.player == i).position;
         
            // If our position is lower (1st is lower than 4th) we are progressing
            if (newPosition < previousPosition)
            {
                progress[i] = "Forwards";
                hasChangedPositions = true;
            }
            // I mean
            else if (newPosition > previousPosition)
            {
                progress[i] = "Backwards";
                hasChangedPositions = true;
            }

            Debug.Log($"player {i} has previous position of {previousPosition} and a new position of {newPosition} with a progression of {progress[i]}");
        }
        

        float time = 0f;

        // Only animate if the positions change
        if (hasChangedPositions)
        {
            // Show the progress of the player
            for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
            {
                if (!string.IsNullOrEmpty(progress[i]))
                {
                    resultsControllers[i].ShowArrow(true, progress[i] == "Forwards");
                }
            }

            // Wait for a bit
            yield return new WaitForSeconds(1f);

            // Get the starting and ending positions
            int playerCount = PlayerManager.PlayerCountScaled;
            Vector3[] startingPositions = new Vector3[playerCount];
            Vector3[] endingPositions = new Vector3[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                startingPositions[OldScores[i].player] = positions[i].position;
                endingPositions[Scores[i].player] = positions[i].position;
            }

            // Lerp the players to the new positions
            time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime;
                for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
                {
                    players[i].transform.position = Vector3.Lerp(startingPositions[i], endingPositions[i], time);
                }
                yield return null;
            }
        }

        // Update ordinals
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            resultsControllers[i].UpdatePosition(Scores.First(s => s.player == i).position);
        }

        //foreach (var resultController in resultsControllers)
        //{
        //    resultController.UpdatePosition(Scores.First(s => s.player == resultController.playerNumber).position);
        //}

        // Animate ordinals appearing in order
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            resultsControllers[Scores[i].player].ShowOrdinal();
            yield return new WaitForSeconds(1f);
        }

        // Hide the progress of the player
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            if (!string.IsNullOrEmpty(progress[i]))
            {
                var playerResultController = resultsControllers[i];
                playerResultController.ShowArrow(false, false);
            }
        }

        // Play the reaction
        //foreach (var score in Scores)
        //{
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            int p = Scores[i].player;
            if (!string.IsNullOrEmpty(progress[p]))
            {
                resultsControllers[p].PlayReaction(progress[p] == "Forwards");
                yield return new WaitForSeconds(1f);
            }
        }

        // Wait for a bit
        yield return new WaitForSeconds(1f);

        // Wait before we finish with the results
        yield return new WaitForSeconds(3f);

        // FINISH HIM
        EventManager.FinishResults();

        // Null... I don't need this but it's a habit
        yield return null;
    }
}