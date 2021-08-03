using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerJoinPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject characterSelect;
    [SerializeField]
    private GameObject pressStart;
    [SerializeField]
    private GameObject playerModel;
    [SerializeField]
    private GameObject isReady;
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

    int currentScreen = 0;

    public bool IsReady => currentScreen == 2;
    public bool HasJoined => currentScreen > 0;
    public UnityEvent onReadyStateChanged;

    private bool hasRegistered = false;

    // Start is called before the first frame update
    void Start()
    {
        RandomIcon();

        if (playerNumber == 0)
        {
            player = PlayerManager.GetPlayer(playerNumber);

            ShowScreen(1);
        }
        else
        {
            ShowScreen(0);
        }
    }

    private void OnEnable()
    {
        if (PlayerManager.HasPlayer(playerNumber))
            player = PlayerManager.GetPlayer(playerNumber);

        if (hasRegistered)
            FindObjectOfType<StartMenuController>().StopBackRegister.Register(this);
    }

    private void OnDisable()
    {
        // Stop player 1 from stopping the start menu from going back
        if (playerNumber == 0)
        {
            FindObjectOfType<StartMenuController>().StopBackRegister.UnRegister(this);
            return;
        }
        // Throw the screen back to the press start screen
        if (PlayerManager.HasPlayer(playerNumber))
        {
            PlayerManager.RemovePlayer(playerNumber);
            ShowScreen(0);
        }

    }

    private float time = 0f;
    // Update is called once per frame
    void Update()
    {
        // If we are the first player, and we are on the ready screen, and we want to go out of the ready state
        if (playerNumber == 0 && currentScreen == 2 && player != null && player.GetButtonDown("Cancel"))
        {
            // We want to unregister us from stopping the start menu controller from going back
            FindObjectOfType<StartMenuController>().StopBackRegister.UnRegister(this);
            hasRegistered = false; // This is to handle the disabling and enabling of the register
            ShowScreen(currentScreen-1); // We want to show the previous screen
            onReadyStateChanged.Invoke(); // Tell everyone we have changed state
        }

        // Character selection
        if (currentScreen == 1 && player != null)
        {
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

            if (player.GetButtonDown("Randomise") || (player.GetButtonTimedPress("Randomise", characterSwitchingTime) && time == 0))
            {
                time = characterSwitchingTime;
                RandomIcon();
            }

            if (player.GetButtonDown("Fire"))
            {
                if (playerNumber == 0)
                {
                    FindObjectOfType<StartMenuController>().StopBackRegister.Register(this);
                    hasRegistered = true;
                }
                ShowScreen(2);
                onReadyStateChanged.Invoke();
            }
        }
        else
        {
            // Allow the buttons to repeat through the characters when held
            time = characterSwitchingTime;
        }

        if (playerNumber == 0) return;

        // Player Joining
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            if (!PlayerManager.HasPlayerId(i) && PlayerManager.FirstFreePosition() == playerNumber)
            {
                if (ReInput.players.GetPlayer(i).GetButtonDown("Start"))
                {
                    player = ReInput.players.GetPlayer(i);
                    PlayerManager.AddPlayer(playerNumber, i);
                    ShowScreen(1);
                    onReadyStateChanged.Invoke();
                }
            }
        }

        // Player Leaving
        if (player != null && player.GetButtonDown("Cancel"))
        {
            if (currentScreen > 0)
            {
                ShowScreen(currentScreen-1);
            }
            if (currentScreen == 0)
            {
                player = null;
                PlayerManager.RemovePlayer(playerNumber);
            }
            onReadyStateChanged.Invoke();
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

    public void ShowScreen(int screen)
    {
        currentScreen = screen;

        // Press start
        if (screen == 0)
        {
            playerModel.SetActive(false);
            characterSelect.SetActive(false);
            pressStart.SetActive(true);
            isReady.SetActive(false);
        }
        // Character select
        else if (screen == 1)
        {
            playerModel.SetActive(true);
            characterSelect.SetActive(true);
            pressStart.SetActive(false);
            isReady.SetActive(false);
        }
        // Ready
        else if (screen == 2)
        {
            playerModel.SetActive(true);
            characterSelect.SetActive(false);
            pressStart.SetActive(false);
            isReady.SetActive(true);
        }
    }

    private void NextIcon()
    {
        currentCharacter++;
        currentCharacter %= characterSprites.Length;
        UpdateIcon();
    }

    private void PreviousIcon()
    {
        currentCharacter--;
        if (currentCharacter < 0) currentCharacter = characterSprites.Length - 1;
        UpdateIcon();
    }

    private void RandomIcon()
    {
        currentCharacter = Random.Range(0, characterSprites.Length);
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        characterImage.sprite = characterSprites[currentCharacter];
        PlayerManager.SetPlayerModel(playerNumber, characterSprites[currentCharacter].name);
    }
}
