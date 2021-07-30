using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static ScoreManager;

public class ResultsManager : MonoBehaviour
{
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

    public int GetPosition(int player)
    {
        return OldScores.First(s => s.player == player).position;
    }

    //public int GetOldWins(int player)
    //{
    //    return oldScores.First(s => s.player == player).wins;
    //}

    public int GetScore(int player, Score[] scores)
    {
        return scores.First(s => s.player == player).score;
    }

    //public void AddScore(int player)
    //{
    //    Scores.First(s => s.player == player).score++;
    //}

    private void Start()
    {
        Invoke("ResultsSequence", 2f);
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
            ResultsSequence();
        }
    }
#endif

    private void ResultsSequence()
    {
        // Sort the scores
        Scores = Scores.OrderBy(s => -s.score).ToArray();
        OldScores = OldScores.OrderBy(s => -s.score).ToArray();

        // Set the ordinal positions
        SetScorePositions(Scores);
        SetScorePositions(OldScores);

        // Increment the counters
        foreach (var resultController in FindObjectsOfType<ResultsController>().OrderBy(r => r.playerNumber))
        {
            resultController.UpdatePosition(OldScores.First(s => s.player == resultController.playerNumber).position);

            int p = resultController.playerNumber;
            int from = GetScore(p, OldScores);
            int to = GetScore(p, Scores);

            resultController.StartIncrementScore(from, to);
        }

        // Start the sort animation
        StartCoroutine(SortScores());

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
        }
    }

    private IEnumerator SortScores()
    {
        // We want to wait for the values to set in
        yield return new WaitForEndOfFrame();

        // We want to wait until they have stopped their incrementing animation
        yield return new WaitUntil(() =>
        {
            foreach (var result in FindObjectsOfType<ResultsController>())
            {
                if (result.IsIncrementing)
                {
                    return false;
                }
            }

            return true;
        });

        ResultsController[] resultsControllers = FindObjectsOfType<ResultsController>();

        // Animate ordinals disappearing
        Vector2[] scales = resultsControllers.Select(r => r.GetPositionSize()).ToArray();
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * 4f;
            for (int i = 0; i < resultsControllers.Length; i++)
            {
                resultsControllers[i].SetPositionSize(Vector2.Lerp(scales[i], Vector2.zero, time));
            }
            yield return null;
        }

        // Check if the positions have changed
        bool hasChangedPositions = false;
        for (int i = 0; i < Scores.Length; i++)
        {
            if (Scores[i] != OldScores[i])
            {
                hasChangedPositions = true;
                break;
            }
        }

        // Only animate if the positions change
        if (hasChangedPositions)
        {
            // Create a dictionary of results, this is so we can sort but keep track of the associated results
            Dictionary<Score, ResultsController> results = new Dictionary<Score, ResultsController>();

            // Add all of the scores
            foreach (var result in FindObjectsOfType<ResultsController>())
            {
                results.Add(Scores.First(s => s.player == result.playerNumber), result);
            }

            // Sort the dictionary by the wins
            results = results.OrderBy(r => r.Key.score).ToDictionary(r => r.Key, r => r.Value);

            // Store the starting positions
            Vector3[] startingPositions = results.Select(r => r.Value.transform.position).ToArray();

            // Set as first sibling. This makes the horizontal layout group sort it, which we will lerp to
            foreach (var result in results)
            {
                result.Value.transform.SetAsFirstSibling();
            }

            // Wait until the horizontal layout group sorts it
            yield return new WaitForEndOfFrame();

            // Store the ending positions
            Vector3[] endingPositions = results.Select(r => r.Value.transform.position).ToArray();

            // Disable the layout group, this is so it doesn't overwrite the position we want it to be on
            var hlg = FindObjectOfType<HorizontalLayoutGroup>();
            hlg.enabled = false;

            // Lerp the ordinal element from its starting position to its ending position
            time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime;
                int index = 0;
                foreach (var result in results)
                {
                    result.Value.transform.position = Vector3.Lerp(startingPositions[index], endingPositions[index], time);
                    index++;
                }
                yield return null;
            }

            // Reenable the layout group, we should be in the correct position by then so this should do nothing visibly
            hlg.enabled = true;
        }

        foreach (var resultController in FindObjectsOfType<ResultsController>())
        {
            resultController.UpdatePosition(Scores.First(s => s.player == resultController.playerNumber).position);
        }

        // Sort by sibling index, this is so we have the first player's position be shown first
        resultsControllers = resultsControllers.OrderBy(r => r.transform.GetSiblingIndex()).ToArray();

        // Animate ordinals appearing in order
        for (int i = 0; i < resultsControllers.Length; i++)
        {
            time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime * 2f;
                resultsControllers[i].SetPositionSize(Vector2.Lerp(Vector2.zero, scales[i], time));
                yield return null;
            }
        }

        // Wait before we finish with the results
        yield return new WaitForSeconds(3f);

        // FINISH HIM
        EventManager.FinishResults();

        // Null... I don't need this but it's a habit
        yield return null;
    }
}