using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public int playerNum;
    public GameObject Hat;
    public bool canStealHat;
    private bool wearingHat = false;

    private void Start()
    {
        canStealHat = true;
        Hat.SetActive(false);
    }
    public void HatEnable()
    {
        wearingHat = true;
        Hat.SetActive(true);
        canStealHat = false;
    }

    public void HatDisable()
    {
        wearingHat = false;
        Hat.SetActive(false);
        StartCoroutine("WaitABit");
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && wearingHat)
        {
            Debug.Log("bonk");
            PlayerCollision otherPlayerCollision = collision.gameObject.GetComponent<PlayerCollision>();
            if(otherPlayerCollision.canStealHat)
            {
                HatDisable();
                otherPlayerCollision.HatEnable();
            }
        }

        if (collision.gameObject.CompareTag("Hat"))
        {
            collision.gameObject.SetActive(false);
            HatEnable();
        }
    }

    private IEnumerator WaitABit()
    {
       yield return new WaitForSeconds(1f);
       canStealHat = true;
    }
}
