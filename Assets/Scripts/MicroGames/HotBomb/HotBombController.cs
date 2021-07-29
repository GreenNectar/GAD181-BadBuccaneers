using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBombController : MonoBehaviour
{
    [SerializeField, Tooltip("How long it will take for the bomb to move between players")]
    private float timeToTransfer = 0.5f;

    [SerializeField, Tooltip("How high the bomb will go when transferring")]
    private float transferHeight = 2f;


    private Vector3 oldPosition;
    private HotBombPlayerController previousPlayer;
    private HotBombPlayerController currentPlayer;

    private bool isTransferring;


    // Start is called before the first frame update
    void Start()
    {
        var players = FindObjectsOfType<HotBombPlayerController>();
        currentPlayer = players[Random.Range(0, players.Length)];

        oldPosition = transform.position;

        StartCoroutine(MoveToNewPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransferring && currentPlayer)
        {
            transform.position = currentPlayer.BombPosition.position;
            transform.eulerAngles = new Vector3(0f, currentPlayer.BombPosition.eulerAngles.y, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTransferring) return; // We only want to transfer once we've finished transferring

        if (other.gameObject.GetComponent<HotBombPlayerController>())
        {
            if (other.gameObject != currentPlayer.gameObject)
            {
                oldPosition = currentPlayer.BombPosition.position;
                previousPlayer = currentPlayer;
                currentPlayer = other.gameObject.GetComponent<HotBombPlayerController>();
                StartCoroutine(MoveToNewPlayer());
            }
        }
    }

    private IEnumerator MoveToNewPlayer()
    {
        isTransferring = true;

        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime / timeToTransfer;

            if (previousPlayer) previousPlayer.holdingWeight = Mathf.Clamp(1f - time, 0f, 1f);
            currentPlayer.holdingWeight = Mathf.Clamp(time, 0f, 1f);

            Vector3 fromPosition = previousPlayer ? previousPlayer.BombPosition.position : oldPosition;
            Quaternion fromRotation = previousPlayer ? previousPlayer.BombPosition.rotation : transform.rotation;
            transform.position = Vector3.Lerp(fromPosition, currentPlayer.BombPosition.position, time) + Vector3.up * transferHeight * Mathf.Sin(time * Mathf.PI);
            Quaternion newRotation = Quaternion.Lerp(fromRotation, currentPlayer.BombPosition.rotation, time);

            transform.eulerAngles = new Vector3(0f, newRotation.eulerAngles.y, 0f);
            yield return null;
        }

        isTransferring = false;
    }
}
