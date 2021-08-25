using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerObject : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        // If the player is not added to the manager, disable this gameobject (we don't want to render anything unnecessary)
        if (playerNumber > PlayerManager.PlayerCount - 1 && PlayerManager.PlayerCount != 0)
        {
            gameObject.SetActive(false);
        }
    }
}
