using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRunController : MonoBehaviour
{
    public CharacterController chaCon;
    public Player player;
    public float turnSpeed;
    public float boatSpeed;
    public float lerpSpeed;
    public Transform boat;
    public float boatLean;
    public float boostSpeed;
    public bool inBooster;

    float currentH = 0f;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        speed = boostSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(inBooster == true)
        {
            speed = boostSpeed;
        }
        else
        {
            speed = boatSpeed;
        }

        if (Mathf.Abs(currentH - player.GetAxis("MoveX")) >= lerpSpeed * Time.deltaTime)
        {
            currentH += (player.GetAxis("MoveX") > currentH ? lerpSpeed : -lerpSpeed) * Time.deltaTime;
        }
        else
        {
            currentH = player.GetAxis("MoveX");
        }
        chaCon.Move(new Vector3(currentH * Time.deltaTime * turnSpeed, 0f, 1f * Time.deltaTime * speed));
        boat.rotation = Quaternion.Euler(0f, 0f, currentH * boatLean);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Booster")
        {
            inBooster = true;
        }
        else
        {
            inBooster = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Booster")
        {
            inBooster = false;
        }
    }
}
