using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !ScoreManager.Instance.HasPlayerEnded(other.GetComponent<MicroGamePlayerController>().PlayerNumber))
        {
            other.GetComponent<Animator>().SetTrigger("Victory");

            ScoreManager.Instance.EndPlayer(other.GetComponent<MicroGamePlayerController>().PlayerNumber);
        }
    }
}
