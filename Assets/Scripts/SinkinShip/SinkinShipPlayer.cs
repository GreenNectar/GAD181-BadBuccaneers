using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SinkinShipPlayer : MonoBehaviour
{

    // variables for player statistics
    public float moveSpeed = 10f;
    //public float turnSpeed = 45f;
    public float jumpHeight = 5f;
    //public int lockPickSkill = 0;

    // the buffed variables you gain from leveling up
    public float currentMoveSpeed;
    public float currentTurnSpeed;
    public float currentJumpHeight;
    //public int currentLockPickSkill;


    // the starting xp, xp needed and current level of player
    /*public float xp = 0;
    public float xpForNextLevel = 10;
    */public int level = 0;

    // upon start, setting variables for player
    private void Start()
    {
        //SetXpForNextLevel();
        SetCurrentMoveSpeed();
       //SetCurrentTurnSpeed();
        SetCurrentJumpHeight();
        //SetCurrentLockPickSkill();
    }

    // to level up player needs an amount of xp, starts at 10
    // every level player needs more xp = 10 + level * level * 0.1
    // factoring level up growth by factor of 10%
    /*void SetXpForNextLevel()
    {
        xpForNextLevel = (10f + (level * level * 0.1f));
        Debug.Log("xpForNextLevel " + xpForNextLevel);
    }
    */

    // For each level, the player adds 10% to the move speed, turn speed, jump height
    void SetCurrentMoveSpeed()
    {
        currentMoveSpeed = this.moveSpeed + (this.moveSpeed * 0.1f * level);
        Debug.Log("currentMoveSpeed = " + currentMoveSpeed);
    }

    /*void SetCurrentTurnSpeed()
    {
        currentTurnSpeed = this.turnSpeed + (this.turnSpeed * (level * 0.1f));
        Debug.Log("currentTurnSpeed = " + currentTurnSpeed);
    }

    */

    void SetCurrentJumpHeight()
    {
        currentJumpHeight = this.jumpHeight + (this.jumpHeight * (level * 0.1f));
        Debug.Log("currentJumpHeight = " + currentJumpHeight);
    }

    // For each level, the player adds their total level to their lockpickskill

    /*void SetCurrentLockPickSkill()
    {
        currentLockPickSkill = this.lockPickSkill + level;
        Debug.Log("currentLockPickSkill = " + currentLockPickSkill);
    }
    */

    // Player leveling up causes current variables to update

   /* void LevelUp()
    {
        xp = 0f;
        level++;
        Debug.Log("level" + level);
        SetXpForNextLevel();
        SetCurrentMoveSpeed();
        SetCurrentTurnSpeed();
        SetCurrentJumpHeight();
        SetCurrentLockPickSkill();

    }
   */

    //a function to make the player gain the amount of Xp the you tell it. 
   /* public void GainXP(int xpToGain)
    {
        xp += xpToGain;
        Debug.Log("Gained " + xpToGain + " XP, Current Xp = " + xp + ", XP needed to reach next Level = " + xpForNextLevel);
    }
   */

    //  Update function every frame
    void Update()
    {
        // when pressing the x button, player gains 1 xp
        //if (Input.GetKeyDown(KeyCode.X) == true) { GainXP(1); }

        //when player reaches -10 on y axis, restarts level
        /*if (transform.position.y < -10f)
        {
            Application.LoadLevel("Snowman");
        }
        */

        // When xp is > or = xp needed for next level, player levels up
       /* if (xp >= xpForNextLevel)
        {
            LevelUp();
        }
       */

        // using up arrow and down arrow, Movement is based on position, equal to forward movement * move speed * time between update frames

        if (Input.GetKey(KeyCode.RightArrow) == true) { this.transform.position += this.transform.forward * currentMoveSpeed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.LeftArrow) == true) { this.transform.position -= this.transform.forward * currentMoveSpeed * Time.deltaTime; }


        if (Input.GetKey(KeyCode.Space) == true && Mathf.Abs(this.GetComponent<Rigidbody>().velocity.y) < 0.001f)
        {
            this.GetComponent<Rigidbody>().velocity += Vector3.up * this.currentJumpHeight;
        }
    }
}



