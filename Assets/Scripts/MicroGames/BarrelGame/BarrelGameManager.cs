using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject barrel;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GameSequence()
    {
        int currentRound = 0;
        while(currentRound < rounds.Length)
        {
            // Get the round
            BarrelRound round = rounds[currentRound];
            // Get a random row
            Row row = rows[Random.Range(0, rows.Length)];
            // Randomise the positions of the row
            Transform[] positions = row.positions.OrderBy(p => Random.value).ToArray();

            // Create the barrels on the positions
            for (int i = 0; i < round.barrels; i++)
            {
                Instantiate(barrel, positions[i].position, positions[i].rotation);
            }

            // Wait a bit until the next barrels get sent
            yield return new WaitForSeconds(round.timeUntilNextRound);

            currentRound++;
        }
    }
}

