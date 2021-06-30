using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControllrt : MonoBehaviour
{
    public CharacterController chaCon;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0); 
    }

    // Update is called once per frame
    void Update()
    {
        chaCon.Move(new Vector3(player.GetAxis("MoveX"), 0f, player.GetAxis("MoveY")) * Time.deltaTime);
    }
}
