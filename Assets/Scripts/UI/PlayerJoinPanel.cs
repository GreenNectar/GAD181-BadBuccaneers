using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJoinPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject characterSelect;
    [SerializeField]
    private GameObject pressStart;
    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private int playerNumber;
    [SerializeField]
    private float characterSwitchingTime = 0.25f;

    private Player player;
    private int currentCharacter;

    [SerializeField]
    private Sprite[] characterSprites;

    //private static bool[] players = new bool[4] { false, false, false, false };

    // Start is called before the first frame update
    void Start()
    {
        if (playerNumber == 0)
        {
            player = PlayerManager.GetPlayer(playerNumber);

            pressStart.SetActive(false);
            characterSelect.SetActive(true);
        }
        else
        {
            characterSelect.SetActive(false);
            pressStart.SetActive(true);
        }
    }

    private void OnEnable()
    {
        if (PlayerManager.HasPlayer(playerNumber))
            player = PlayerManager.GetPlayer(playerNumber);
    }

    private void OnDisable()
    {
        if (playerNumber == 0) return;
        if (PlayerManager.HasPlayer(playerNumber))
        {
            PlayerManager.RemovePlayer(playerNumber);
            characterSelect.SetActive(false);
            pressStart.SetActive(true);
        }
    }

    private float time = 0f;
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //float axis = player.GetAxis("LeftMoveX");
            //if (axis != 0.25f)
            //{

            //}

            time -= Time.deltaTime;
            time = Mathf.Clamp(time, 0f, characterSwitchingTime);

            if (player.GetButtonDown("Left") || (player.GetButtonTimedPress("Left", characterSwitchingTime) && time == 0))
            {
                time = characterSwitchingTime;
                PreviousIcon();
            }

            if (player.GetButtonDown("Right") || (player.GetButtonTimedPress("Right", characterSwitchingTime) && time == 0))
            {
                time = characterSwitchingTime;
                NextIcon();
            }
        }

        if (playerNumber == 0) return;

        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            if (!PlayerManager.HasPlayerId(i) && PlayerManager.FirstFreePosition() == playerNumber)
            {
                if (ReInput.players.GetPlayer(i).GetButtonDown("Start"))
                {
                    player = ReInput.players.GetPlayer(i);
                    PlayerManager.AddPlayer(playerNumber, i);

                    characterSelect.SetActive(true);
                    pressStart.SetActive(false);
                }
            }
        }

        if (player != null && player.GetButtonDown("Cancel"))
        {
            player = null;
            PlayerManager.RemovePlayer(playerNumber);

            characterSelect.SetActive(false);
            pressStart.SetActive(true);
        }

        //if (!PlayerManager.HasPlayer(playerNumber) && playerNumber <= PlayerManager.PlayerCount)
        //{
        //    for (int i = 0; i < ReInput.players.playerCount; i++)
        //    {
        //        if (ReInput.players.GetPlayer(i).GetButtonDown("Start"))
        //        {
        //            player = ReInput.players.GetPlayer(i);
        //            PlayerManager.AddPlayer(playerNumber, i);

        //            characterSelect.SetActive(true);
        //            pressStart.SetActive(false);
        //        }
        //    }
        //}

        //if (PlayerManager.HasPlayer(playerNumber))
        //{
        //    if (player == null || player.GetButtonDown("Cancel"))
        //    {
        //        player = null;
        //        PlayerManager.RemovePlayer(playerNumber);

        //        characterSelect.SetActive(false);
        //        pressStart.SetActive(true);
        //    }
        //}
    }

    private void NextIcon()
    {
        currentCharacter++;
        currentCharacter %= characterSprites.Length;
        characterImage.sprite = characterSprites[currentCharacter];
    }

    private void PreviousIcon()
    {
        currentCharacter--;
        if (currentCharacter < 0) currentCharacter = characterSprites.Length - 1;
        characterImage.sprite = characterSprites[currentCharacter];
    }
}
