using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDebris : MonoBehaviour
{
    public GameObject[] theDebris;

    // Positions for the parrots to spawn at
    public float xPos;
    public float yPos;
    public float zPos;
    public float debrisYRotation;

    // Amount and time for enemy to spawn at
    public int debrisCount;
    public int debrisMax;
    public float debrisSpawnTime;




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DebrisDrop());
    }

    // Spawns an enemy every enemySpawnTime seconds at a position, at a rotation until the max amount of enemies in the level have spawned
    IEnumerator DebrisDrop()
    {
        while (debrisCount < debrisMax)
        {
            //yPos = Random.Range(4, 20);
            debrisSpawnTime = Random.Range(1, 3);
            Instantiate(theDebris[Random.Range(0, 3)], new Vector3(xPos, yPos, zPos), Quaternion.Euler(0, debrisYRotation, 0));
            yield return new WaitForSeconds(debrisSpawnTime);
            debrisCount += 1;
        }
    }
}
