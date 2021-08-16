using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.InputSystem;
//using static UnityEngine.InputSystem.InputAction;

public class FishingController : MicroGamePlayerController
{
    [Tooltip("Player Values")]
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float position = 0f;
    [SerializeField]
    private Line line;
    public float startingHeight = 0f;
    public float fishSpawnDepth = 0.5f;
    public float depth = 5f;

    [Tooltip("Fish")]
    [SerializeField]
    private Transform bucket;
    [SerializeField]
    private int maxFishStackHeight = 10;

    [Tooltip("Events")]
    public UnityEvent onAddFish;

    private float movement;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // Add this controller to the manager
        FindObjectOfType<FishingManager>().Add(this);
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
        if (!line.hook.activeSelf && position == 0f)
        {
            line.hook.SetActive(true);
        }
    }

    private void GetInput()
    {
        movement = player.GetAxis("LeftMoveY");
    }

    /// <summary>
    /// Adds an object to the players bucket
    /// </summary>
    /// <param name="fish"></param>
    public void AddFish(GameObject fish)
    {
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

        onAddFish.Invoke();
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
}
