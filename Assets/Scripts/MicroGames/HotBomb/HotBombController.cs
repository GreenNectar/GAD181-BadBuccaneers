using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Pulse))]
public class HotBombController : MonoBehaviour
{
    [SerializeField, Tooltip("How long it will take for the bomb to move between players")]
    private float timeToTransfer = 0.5f;

    [SerializeField, Tooltip("How high the bomb will go when transferring")]
    private float transferHeight = 2f;

    [SerializeField, Tooltip("How long until the bomb starts to throb")]
    private float timeUntilFirstThrob = 10;
    [SerializeField, Tooltip("How long until the bomb starts to throb harshly, from when it started throbbing")]
    private float timeUntilSecondThrob = 6;
    [SerializeField, Tooltip("How long from when it starts throbbing will it explode")]
    private float timeUntilExplode = 4;
    [SerializeField]
    private float randomTimeOffset = 4;
    [SerializeField]
    private float timeUntilNextRound = 3;

    [SerializeField]
    private ParticleSystem bombFX;

    // Position switching and player management
    private Vector3 initialPosition;
    private Vector3 oldPosition;
    private Quaternion oldRotation;
    private HotBombPlayerController previousPlayer;
    private HotBombPlayerController currentPlayer;

    // Extra stuffing
    private Pulse pulse;

    private bool isTransferring;

    private bool allowVibration;
    private bool isHarshVibration;


    // Start is called before the first frame update
    void Start()
    {
        pulse = GetComponent<Pulse>();

        SetRandomPlayer();

        initialPosition = transform.position;
        oldPosition = transform.position;
        oldRotation = transform.rotation;

        StartCoroutine(MoveToNewPlayer());

        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransferring && currentPlayer)
        {
            transform.position = currentPlayer.BombPosition.position;
            transform.eulerAngles = new Vector3(0f, currentPlayer.BombPosition.eulerAngles.y, 0f);
        }

        // Vibrate the controller of the person holding based on the pulse's sine time
        if (currentPlayer && allowVibration)
        {
            if (pulse.enabled && !isHarshVibration)
            {
                Vibrator.Instance.Vibrate(currentPlayer.PlayerNumber, 1, pulse.AbsoluteSineTime);
            }
            else
            {
                Vibrator.Instance.Vibrate(currentPlayer.PlayerNumber, 1, isHarshVibration ? 1f : 0.25f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTransferring) return; // We only want to transfer once we've finished transferring

        if (other.gameObject.GetComponent<HotBombPlayerController>() && other.gameObject.GetComponent<HotBombPlayerController>().enabled)
        {
            if (other.gameObject != currentPlayer.gameObject)
            {
                oldPosition = currentPlayer.BombPosition.position;
                oldRotation = currentPlayer.BombPosition.rotation;
                previousPlayer = currentPlayer;
                Vibrator.Instance.Vibrate(currentPlayer.PlayerNumber, 0, 0f);
                Vibrator.Instance.Vibrate(currentPlayer.PlayerNumber, 1, 0f);
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
            if (currentPlayer == null)
            {
                break;
            }

            time += Time.deltaTime / timeToTransfer;

            if (previousPlayer) previousPlayer.holdingWeight = Mathf.Clamp(1f - time, 0f, 1f);
            currentPlayer.holdingWeight = Mathf.Clamp(time, 0f, 1f);

            //Vector3 fromPosition = previousPlayer ? previousPlayer.BombPosition.position : oldPosition;
            //Quaternion fromRotation = previousPlayer ? previousPlayer.BombPosition.rotation : transform.rotation;

            transform.position = Vector3.Lerp(oldPosition, currentPlayer.BombPosition.position, time) + Vector3.up * transferHeight * Mathf.Sin(time * Mathf.PI);
            Quaternion newRotation = Quaternion.Lerp(oldRotation, currentPlayer.BombPosition.rotation, time);

            transform.eulerAngles = new Vector3(0f, newRotation.eulerAngles.y, 0f);
            yield return null;
        }

        isTransferring = false;
    }

    private IEnumerator CountDown()
    {
        allowVibration = true;

        // Start the throb
        yield return new WaitForSeconds(timeUntilFirstThrob + Random.Range(-randomTimeOffset / 2f, randomTimeOffset / 2f));
        pulse.enabled = true;

        // Start the harsh throb
        yield return new WaitForSeconds(timeUntilSecondThrob);
        float initialThrobCutoff = pulse.pulseCutoff;
        pulse.pulseCutoff = 0f;
        pulse.pulseSpeed *= 2f;
        isHarshVibration = true;

        // Show explode fx
        yield return new WaitForSeconds(timeUntilExplode);
        Instantiate(bombFX, transform.position, Quaternion.identity);
        isHarshVibration = false;
        allowVibration = false;

        // Feel the explosion!
        Vibrator.Instance.ImpactVbration(currentPlayer.PlayerNumber, 0, 0.5f);
        Vibrator.Instance.ImpactVbration(currentPlayer.PlayerNumber, 1, 0.5f);

        yield return new WaitForSeconds(0.2f); // Allow the fx to go for a bit

        currentPlayer.Kill();
        currentPlayer = null;
        StopCoroutine(MoveToNewPlayer()); // We want to stop swapping!!! (this was a glitch!)

        // Stop the previous player from holding (in case we are mid swap!)
        if (previousPlayer != null)
        {
            previousPlayer.holdingWeight = 0f;
            previousPlayer.SetHolding();
        }

        // Set the bomb back to where it started
        transform.position = initialPosition;
        oldPosition = transform.position;
        oldRotation = transform.rotation;
        // Reset the previous player
        previousPlayer = null;

        // Reset the pulsing
        pulse.ResetScale();
        pulse.pulseSpeed /= 2f;
        pulse.pulseCutoff = initialThrobCutoff;
        pulse.enabled = false;

        yield return new WaitForSeconds(timeUntilNextRound);

        // If there is more than one player, start the process again
        if (FindObjectsOfType<HotBombPlayerController>().Where(p => p.enabled).Count() > 1)
        {
            SetRandomPlayer(true); // Get a whole new random player
            StartCoroutine(MoveToNewPlayer()); // Move to the player
            StartCoroutine(CountDown());
        }
        // Otherwise end the game
        else
        {
            yield return new WaitForSeconds(0.5f);

            ScoreManager.Instance.EndFinalPlayers();

            yield return new WaitForSeconds(2.5f);

            GameManager.Instance.EndGame();
        }
    }

    private void SetRandomPlayer(bool checkEnabled = false)
    {
        var players = FindObjectsOfType<HotBombPlayerController>().Where(p => p.enabled || !checkEnabled).ToArray();
        currentPlayer = players[Random.Range(0, players.Length)];
    }
}