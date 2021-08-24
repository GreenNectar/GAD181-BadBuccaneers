using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotBehaviour : MonoBehaviour
{
    public int speed = 5;
    public float waitTime = 2;
    public float destroyTime = 6f;
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
        Destroy(gameObject, destroyTime);
    }
    IEnumerator OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collison");
        if (collider.gameObject.CompareTag("Player"))
        {
            FlagMovement moveScript = collider.GetComponent<FlagMovement>();
            moveScript.canMove = false;

            yield return new WaitForSeconds(waitTime);

            moveScript.canMove = true;

        }

    }



}
