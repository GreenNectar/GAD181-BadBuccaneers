using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotBehaviour : MonoBehaviour
{

    public int speed = 5;
    public float knockback;
    public float knockbackTime;
  
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

            //collision.gameObject.transform.position -= transform.up * knockback;
           
                //When this object collides with Player, check if player has component (health)
                if (collision.gameObject.GetComponent<FlagMovement>() == true)
                {
                    //If player has Health, take biteDamage from health
                    collision.gameObject.GetComponent<FlagMovement>().TakeDamage(knockbackTime);
                }


        }

    }
   
}
