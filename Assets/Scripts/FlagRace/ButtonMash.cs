using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMash : MonoBehaviour
{

    public float mashDelay = .5f;
    public float flagSpeed = 1f;
    public float dropSpeed = 0.5f;
    public int knockback;

    float mash;
    bool pressed;
    bool started;

    // Start is called before the first frame update
    void Start()
    {
        mash = mashDelay;
    }

    // Update is called once per frame
    // flag moves up with each space bar
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
        }

        if (started)
        {
            //text.SetActive(true);
            mash -= Time.deltaTime;

            
            // if the space bar is not pressed within the delay time, the flag moves down
            if (Input.GetKeyDown(KeyCode.Space) && !pressed)
            {
                pressed = true;
                mash = mashDelay;
                this.transform.position += transform.up * flagSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                pressed = false;
            }

            //Flag moves down
            if (mash <= 0)
            {
                this.transform.position -= transform.up * dropSpeed;
            }
           
        }
    }
}
