using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerJoinController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI readyToPlay;
    [SerializeField]
    private TextMeshProUGUI requiresTwoPlayers;

    [SerializeField]
    private PlayerJoinPanel[] panels;

    [SerializeField]
    private StartMenuController startMenuController;

    private bool isReady;

    private void OnEnable()
    {
        foreach (var panel in panels)
        {
            panel.onReadyStateChanged.AddListener(UpdateReady);
        }
        UpdateReady();
    }

    private void OnDisable()
    {
        foreach (var panel in panels)
        {
            panel.onReadyStateChanged.RemoveListener(UpdateReady);
        }
    }

    private void Update()
    {
        if (isReady && PlayerManager.GetPlayer(0).GetButtonDown("Submit"))
        {
            startMenuController.MoveTo("FinalLaunch");
        }
    }

    private void UpdateReady()
    {
        isReady = PlayerManager.PlayerCount > 1;
        foreach (var panel in panels)
        {
            if (!panel.IsReady && panel.HasJoined)
            {
                isReady = false;
            }
        }

        readyToPlay.gameObject.SetActive(isReady);
        requiresTwoPlayers.gameObject.SetActive(PlayerManager.PlayerCount < 2);
    }
}
