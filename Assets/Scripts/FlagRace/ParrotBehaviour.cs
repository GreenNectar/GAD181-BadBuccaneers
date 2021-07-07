using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotBehaviour : MonoBehaviour
{

    public int speed = 5;
    public float knockback;
  
    // Update is called once per frame
    // Moves the parrots across the screen smoothly
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    // When a parrot collides with the player/flag, knocks them down the pole
   void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collison");
        // Check if collision is with Player
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position -= transform.up * knockback;


        }

    }
   
}
