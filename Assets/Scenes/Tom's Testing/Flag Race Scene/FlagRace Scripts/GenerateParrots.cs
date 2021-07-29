using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateParrots : MonoBehaviour
{


    public GameObject theEnemy;

    // Positions for the parrots to spawn at
    public int xPos = -15;
    public int yPos;
    public float zPos = -1.5f;
    public float enemyYRotation;

    // Amount and time for enemy to spawn at
    public int enemyCount;
    public int enemyMax;
    public float enemySpawnTime;




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    // Spawns an enemy every enemySpawnTime seconds at a position, at a rotation until the max amount of enemies in the level have spawned
    IEnumerator EnemyDrop()
    {
        while (enemyCount < enemyMax)
        {
            yPos = Random.Range(0, 20);
            Instantiate(theEnemy, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0, enemyYRotation, 0));
            yield return new WaitForSeconds(enemySpawnTime);
            enemyCount += 1;
        }
    }
   
}
