using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFGenerateShips : MonoBehaviour
{
    public GameObject playerShip;
    public int shipCount;

    public int xPos;
    public int zPos;
    public int xMax;
    public int xMin;
    public int zMax;
    public int zMin;

    void Start()
    {
        ShipSpawn();
    }

    void ShipSpawn()
    {
        while (shipCount < 1)
        {
            xPos = Random.Range(xMin, xMax);
            zPos = Random.Range(zMin, zMax);
            Instantiate(playerShip, new Vector3(xPos, 3, zPos), Quaternion.identity);
            shipCount += 1;
        }
    }
    
}
