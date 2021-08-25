using FMOD.Studio;
using FMODUnity;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.InputSystem;
//using static UnityEngine.InputSystem.InputAction;

public class FishingController : MicroGamePlayerController
{
    [Header("Object References")]
    [SerializeField]
    private Animator animator;

    [Header("Player Values")]
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float position = 0f;
    [SerializeField]
    private Line line;
    public float startingHeight = 0f;
    public float fishSpawnDepth = 0.5f;
    public float depth = 5f;

    [Header("Fish")]
    [SerializeField]
    private GameObject hook;
    [SerializeField]
    private Transform bucket;
    [SerializeField]
    private int maxFishStackHeight = 10;

    [Header("Audio")]
    [SerializeField, EventRef]
    private string fishAddEvent;
    [SerializeField, EventRef]
    private string throwLineEvent;
    [SerializeField, EventRef]
    private string reelingEvent;
    private EventInstance reelingInstance;

    private float movement;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // Add this controller to the manager
        FindObjectOfType<FishingManager>().Add(this);

        // Throw line sound
        RuntimeManager.PlayOneShot(throwLineEvent, transform.position);

        // Create reeling instance
        reelingInstance = RuntimeManager.CreateInstance(reelingEvent);
        reelingInstance.start();
    }

    private void OnDestroy()
    {
        reelingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        reelingInstance.release();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        // Player movement
        position -= movement * Time.deltaTime * speed;
        position = Mathf.Clamp(position, 0f, depth);
        line.transform.position = new Vector3(line.transform.position.x, startingHeight - position, line.transform.position.z);

        // If the hook is broken, and we are at the top, fix the hook
        if (position == 0f)
        {
            ReplaceHook();
        }
    }

    private void GetInput()
    {
        movement = player.GetAxis("LeftMoveY");

        reelingInstance.setParameterByName("ReelingIn", Mathf.Clamp(movement * 10f, -1f, 1f));
    }

    /// <summary>
    /// Adds an object to the players bucket
    /// </summary>
    /// <param name="fish"></param>
    public void AddFish(GameObject fish)
    {
        RuntimeManager.PlayOneShot(fishAddEvent, hook.transform.position);

        ScoreManager.Instance.AddScoreToPlayer(PlayerNumber, 1);

        if (fish.GetComponent<Animator>())
        {
            fish.GetComponent<Animator>().enabled = false;
        }

        fish.transform.parent = bucket;
        fish.transform.localPosition = Vector3.zero;
        fish.transform.rotation = bucket.rotation;
        fish.transform.position += Vector3.up * ((bucket.childCount - 1) % maxFishStackHeight) * 0.1f;
        fish.transform.position += Vector3.forward * Mathf.Floor((bucket.childCount - 1) / maxFishStackHeight) * 0.25f;
    }

    /// <summary>
    /// Removes all fish from the bucket
    /// </summary>
    public void ClearFish()
    {
        for (int i = 0; i < bucket.childCount; i++)
        {
            Destroy(bucket.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Draw the starting position and depth, basically the min/max of the hook
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }

    [SerializeField]
    private bool drawGizmos = false;
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            DrawGizmos();
        }
    }

    private void DrawGizmos()
    {
        Vector3 starting = line.transform.position;
        starting.y = startingHeight;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(starting, 0.25f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(starting + Vector3.down * fishSpawnDepth, 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(starting + Vector3.down * depth, 0.25f);
    }

    public void RemoveHook()
    {
        if (hook.activeSelf)
        {
            hook.SetActive(false);
            PlayerManager.GetPlayerFMODEvent(PlayerNumber).Sad(animator.gameObject);
            animator.SetTrigger("Upset");
        }
    }

    public void ReplaceHook()
    {
        if (!hook.activeSelf)
        {
            hook.SetActive(true);
            animator.SetTrigger("Cast");

            // Throw line sound
            RuntimeManager.PlayOneShot(throwLineEvent, transform.position);
        }
    }
}
