using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDeath : MonoBehaviour
{
    public GameObject[] theBalls;

    public float xPos;
    public float yPos;
    public float zPos;

    // Amount and time for enemy to spawn at
    public int ballCount;
    


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.collider.gameObject);
    }

    private void Update()
    {
        ballCount = GameObject.FindGameObjectsWithTag("CannonBall").Length;

        while (ballCount < 1)
        {
            Instantiate(theBalls[Random.Range(0, 3)], new Vector3(xPos, yPos, zPos), Quaternion.Euler(0, 0, 0));
            ballCount++;
        }
    }

}
