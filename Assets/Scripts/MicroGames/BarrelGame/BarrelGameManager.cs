using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelGameManager : MonoBehaviour
{
    [SerializeField]
    private BarrelController barrel;

    [SerializeField]
    private Row[] rows;

    [SerializeField]
    private BarrelRound[] rounds;

    [Serializable]
    private class BarrelRound
    {
        [Range(1, 5)]
        public int barrels = 1;
        public float speed = 1f;
        public float timeUntilNextRound = 2f;
    }

    [Serializable]
    private class Row
    {
        public Transform[] positions;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameSequence());
    }

    private IEnumerator GameSequence()
    {
        int currentRound = 0;
        BarrelGamePlayerController[] players = FindObjectsOfType<BarrelGamePlayerController>();
        while(currentRound < rounds.Length)
        {
            if (players.Where(p => p.hasWipedOut).Count() == PlayerManager.PlayerCountScaled)
            {
                break;
            }

            // Get the round
            BarrelRound round = rounds[currentRound];
            // Get a random row
            Row row = rows[Random.Range(0, rows.Length)];
            // Randomise the positions of the row
            Transform[] positions = row.positions.OrderBy(p => Random.value).ToArray();

            // Create the barrels on the positions
            for (int i = 0; i < round.barrels; i++)
            {
                Instantiate(barrel, positions[i].position, positions[i].rotation).speed = round.speed;
            }

            // Wait a bit until the next barrels get sent
            yield return new WaitForSeconds(round.timeUntilNextRound);

            currentRound++;
        }

        // Wait for all barrels to fuck off
        yield return new WaitUntil(() => FindObjectOfType<BarrelController>() == null);

        ScoreManager.Instance.EndFinalPlayers();

        yield return new WaitForSeconds(3f);

        // ENd theA _F a_Fijao fj
        GameManager.EndGameStatic();
    }
}

