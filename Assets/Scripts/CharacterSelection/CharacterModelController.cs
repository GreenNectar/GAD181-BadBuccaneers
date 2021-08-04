using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelController : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;

    [SerializeField]
    private GameObject[] skins;

    private void Start()
    {
        string playerSkin = PlayerManager.GetPlayerModel(playerNumber);
        if (!string.IsNullOrEmpty(playerSkin))
        {
            // If a skin is selected, enable the corresponding object while disabling all others
            foreach (var skin in skins)
            {
                if (skin.name.ToLower() != playerSkin.ToLower())
                {
                    skin.SetActive(false);
                }
                else
                {
                    skin.SetActive(true);
                }
            }
        }
        else
        {
            // If no skin is selected, default to the first skin
            foreach (var skin in skins)
            {
                skin.SetActive(false);
            }
            skins[0].SetActive(true);
        }
    }
}
