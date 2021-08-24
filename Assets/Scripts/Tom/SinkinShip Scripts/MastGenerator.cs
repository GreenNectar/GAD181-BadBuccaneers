using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastGenerator : MonoBehaviour
{


    public GameObject mast;

    // Positions for the parrots to spawn at
    public int yPos = -15;
    public int xPos;
    public int xPosMax;
    public float zPos = -1.5f;
    public float enemyYRotation;

    // Amount and time for enemy to spawn at
    public int mastCount;
    public int mastMax;
    public float mastSpawnTime;




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MastDrop());
    }

    // Spawns an enemy every enemySpawnTime seconds at a position, at a rotation until the max amount of enemies in the level have spawned
    IEnumerator MastDrop()
    {
        while (mastCount < mastMax)
        {
            xPos = Random.Range(xPosMax, 0);
            Instantiate(mast, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0, enemyYRotation, 0));
            yield return new WaitForSeconds(mastSpawnTime);
            mastCount += 1;
        }
    }

}