using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//TODO Unyuckify this
public class PlayerJoin : MonoBehaviour
{
    public UnityEvent onPlayer1Join;
    public UnityEvent onPlayer1Leave;
    public UnityEvent onPlayer2Join;
    public UnityEvent onPlayer2Leave;
    public UnityEvent onPlayer3Join;
    public UnityEvent onPlayer3Leave;
    public UnityEvent onPlayer4Join;
    public UnityEvent onPlayer4Leave;

    public int playerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PlayerManager.PlayerCount; i++)
        {
            switch (i)
            {
                case 0:
                    onPlayer1Join.Invoke();
                    break;
                case 1:
                    onPlayer2Join.Invoke();
                    break;
                case 2:
                    onPlayer3Join.Invoke();
                    break;
                case 3:
                    onPlayer4Join.Invoke();
                    break;
                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ReInput.isReady)
        {
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                if (ReInput.players.GetPlayer(i).GetButtonDown("Start"))
                {
                    Debug.Log($"Player {i} pressed start");
                    if (!PlayerManager.HasPlayerId(i))
                    {
                        if (PlayerManager.PlayerCount < 4)
                        {
                            PlayerManager.AddPlayer(PlayerManager.FirstFreePosition(), i);
                        }
                    }

                    switch (PlayerManager.GetPlayerNumber(i))
                    {
                        case 0:
                            onPlayer1Join.Invoke();
                            break;
                        case 1:
                            onPlayer2Join.Invoke();
                            break;
                        case 2:
                            onPlayer3Join.Invoke();
                            break;
                        case 3:
                            onPlayer4Join.Invoke();
                            break;
                        default:
                            break;
                    }
                }

                if (ReInput.players.GetPlayer(i).GetButtonDown("Cancel"))
                {
                    Debug.Log($"Player {i} pressed cancel");
                    if (PlayerManager.HasPlayerId(i))
                    {
                        int playerNumber = PlayerManager.GetPlayerNumber(i);

                        PlayerManager.RemovePlayer(playerNumber);

                        switch (playerNumber)
                        {
                            case 0:
                                onPlayer1Leave.Invoke();
                                break;
                            case 1:
                                onPlayer2Leave.Invoke();
                                break;
                            case 2:
                                onPlayer3Leave.Invoke();
                                break;
                            case 3:
                                onPlayer4Leave.Invoke();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            
        }
    }
}
