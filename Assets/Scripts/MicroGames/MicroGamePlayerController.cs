using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MicroGamePlayerController : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField]
    private int playerNumber = 0;

    public int PlayerNumber => playerNumber;

    protected Player player;

    protected virtual void Start()
    {
        // Get the player
        player = ReInput.players.GetPlayer(PlayerManager.GetPlayerId(playerNumber));
    }
}