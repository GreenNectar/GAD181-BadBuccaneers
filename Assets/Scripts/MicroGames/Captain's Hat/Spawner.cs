using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    GameObject[] players;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        int num = players.Length;

        for (int i = 0; i < num; i++)
        {

            /* Distance around the circle */
            float radians = 2 * Mathf.PI / num * i;


            /* Get the vector direction */
            float vertical = Mathf.Sin(radians);
            float horizontal = Mathf.Cos(radians);

            Vector3 spawnDir = new Vector3(horizontal, 0.5f, vertical);

            /* Get the spawn position */
            Vector3 spawnPos = spawnDir * radius; // Radius is just the distance away from the point

            Debug.Log(spawnPos);
            players[i].transform.position = spawnPos;
        }
    }
}
