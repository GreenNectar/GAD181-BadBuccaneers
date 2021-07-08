using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMovement : MonoBehaviour
{
    [SerializeField]
    private int playerNumber = 0;
    private Player player;

  //  [SerializeField]
  //  private float position = 0f;

    public float mashDelay = .5f;
    public float flagSpeed = 1f;
    public float dropSpeed = 0.5f;
    public int knockback;
    public float verticalSpeed;
    public float topHeight;

    float mash;
    bool pressed;
    bool started;
    
    // script for disabling flag input
    public float waitTime;
    bool doneWaiting = false;
    public bool interactable = true;


    // Start is called before the first frame update
    void Start()
    {
        // Get the player
        player = ReInput.players.GetPlayer(playerNumber);
        mash = mashDelay;
    }

    // increasing wait time when hit by parrot
    public void TakeDamage(float DamageToTake)
    {
        mash -= DamageToTake;
    }

        // Update is called once per frame
        // flag moves up with each space bar
        public void Update()
    {

        // Wait until waitTime is below or equal to zero.
        if (mash > 0)
        {
            mash -= Time.deltaTime;
            //SetActive .interactable = false;
        }

        else
        {
            // Done.
            doneWaiting = true;

        }

        // Only proceed if doneWaiting is true.
        if (doneWaiting)
        {
            //ButtonMash.interactable = true;
        }

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

            // This clamps the flag on the y axis
            Vector3 clampedPosition = transform.position;
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, 3f, topHeight);
            transform.position = clampedPosition;

        }


    }
}
