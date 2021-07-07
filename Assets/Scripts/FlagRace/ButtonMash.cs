using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMash : MonoBehaviour
{
    [SerializeField]
    private int playerNumber = 0;
    private Player player;

    public float mashDelay = .5f;
    public float flagSpeed = 1f;
    public float dropSpeed = 0.5f;
    public int knockback;
    public float verticalSpeed;

    float mash;
    bool pressed;
    bool started;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player
        player = ReInput.players.GetPlayer(playerNumber);
        mash = mashDelay;
    }


    // Update is called once per frame
    // flag moves up with each space bar
    void Update()
    {
        if (player.GetButtonDown("SouthButtonVertical"))
        {
            started = true;
        }

        if (started)
        {
            //text.SetActive(true);
            mash -= Time.deltaTime;

            
            // if the space bar is not pressed within the delay time, the flag moves down
            if (player.GetButtonDown("SouthButtonVertical") && !pressed)
            {
                pressed = true;
                mash = mashDelay;
                // transform.position += verticalSpeed * flagSpeed;
                verticalSpeed = flagSpeed;
            }
            else if (player.GetButtonDown("SouthButtonVertical"))
            {
                pressed = false;
            }

            //Flag moves down
            if (mash <= 0)
            {
                // transform.position -= transform.up * dropSpeed;
                verticalSpeed -= dropSpeed * Time.deltaTime;
            }

            transform.position += transform.up * verticalSpeed * Time.deltaTime;

        }


    }
}
