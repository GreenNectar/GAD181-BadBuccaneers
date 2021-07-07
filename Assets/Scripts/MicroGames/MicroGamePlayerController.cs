using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroGamePlayerController : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField]
    private int playerNumber = 0;
    protected Player player;

    protected virtual void Start()
    {
        // Get the player
        player = ReInput.players.GetPlayer(PlayerManager.GetPlayerId(playerNumber));

        // If the player is not added to the manager, disable this gameobject (we don't want to render anything unnecessary)
        if (playerNumber > PlayerManager.PlayerCount - 1 && PlayerManager.PlayerCount != 0)
        {
            gameObject.SetActive(false);
        }
    }
}