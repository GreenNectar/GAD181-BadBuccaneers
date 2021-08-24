using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIconController : MonoBehaviour
{
    [SerializeField]
    private Image playerIcon;

    [SerializeField]
    private Sprite[] playerIcons;

    [SerializeField]
    private int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        // We want to remove this icon if the player isn't joined
        if (!PlayerManager.HasPlayer(playerNumber)) gameObject.SetActive(false);

        playerIcon.sprite = playerIcons.First(p => p.name.ToLower() == PlayerManager.GetPlayerModel(playerNumber).ToLower());
    }
}
