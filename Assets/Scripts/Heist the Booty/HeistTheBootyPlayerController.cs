using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;

public class HeistTheBootyPlayerController : MicroGamePlayerController
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private HeistTheBootyController bootyController;
    [SerializeField]
    private ParticleSystem coinsFX;
    [SerializeField]
    private Footsteps footsteps;

    [SerializeField]
    private Transform fleeingPoint;
    [SerializeField]
    private Transform lootingPoint;

    private NavMeshAgent agent;
    private Vector3[] corners;
    private int previousChickenScore = 0; // This is so you keep the points you chickened out with

    public bool IsReadyToLoot { get; private set; } = false;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        transform.position = fleeingPoint.position + Vector3.back * PlayerNumber * 0.5f;
        footsteps.enabled = false;
        MoveBackToLoot();
    }


    void Update()
    {
        if (player.GetButtonDown("ChickenOut") && bootyController.CanChicken)
        {
            if (bootyController.ChickenOut(PlayerNumber))
            {
                StartCoroutine(MoveToFleePoint());
                previousChickenScore = ScoreManager.Instance.GetScore(PlayerNumber);
            }
        }
    }

    public void GetShot()
    {
        IsReadyToLoot = false;
        coinsFX.Play();
        ScoreManager.Instance.SetPlayerScore(PlayerNumber, previousChickenScore);
        animator.SetTrigger("FuckingDie");
        Vibrator.Instance.ImpactVbration(PlayerNumber, 0, 0.5f);
        Vibrator.Instance.ImpactVbration(PlayerNumber, 1, 0.5f);
        PlayerManager.GetPlayerFMODEvent(PlayerNumber).Death(gameObject);
    }

    public void MoveBackToLoot()
    {
        StartCoroutine(MoveBackToLootSequence());
    }

    public void Loot()
    {
        animator.SetTrigger("Loot");
    }

    public void RunToStart()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToFleePoint());
    }

    private IEnumerator MoveToFleePoint()
    {
        IsReadyToLoot = false;
        footsteps.enabled = true;

        // Start moving
        animator.SetBool("Running", true);

        yield return new WaitForSeconds(0.33f);

        // Calculate path
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(fleeingPoint.position + Vector3.back * PlayerNumber * 0.5f, path);
        corners = path.corners;//agent.path.corners;
        int currentCorner = 0;
        agent.isStopped = true;

        // Look towards the next corner
        while (Vector3.Distance(lootingPoint.position, transform.position) > 0.1f && currentCorner < corners.Length)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(corners[currentCorner] - transform.position, Vector3.up), Time.deltaTime * 8f);

            if (Vector3.Distance(corners[currentCorner], transform.position) < 0.1f)
            {
                currentCorner++;
            }

            yield return null;
        }

        footsteps.enabled = false;

        // Stop moving
        animator.SetBool("Running", false);
    }

    private IEnumerator MoveBackToLootSequence()
    {
        // Getup if we are downed
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Flying Back Death")) animator.SetTrigger("GetUp");

        yield return new WaitForSeconds(0.5f);

        footsteps.enabled = true;

        // Start moving
        animator.SetBool("Running", true);

        // Calculate path
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(lootingPoint.position, path);
        corners = path.corners;//agent.path.corners;
        int currentCorner = 0;
        agent.isStopped = true;

        // Look towards the next corner
        while(Vector3.Distance(lootingPoint.position, transform.position) > 0.1f && currentCorner < corners.Length)
        {
            Debug.DrawLine(transform.position, corners[currentCorner], Color.white);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(corners[currentCorner] - transform.position, Vector3.up), Time.deltaTime * 8f);

            if (Vector3.Distance(corners[currentCorner], transform.position) < 0.1f)
            {
                currentCorner++;
            }

            yield return null;
        }

        // Stop moving
        animator.SetBool("Running", false);
        Quaternion wantedRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up);
        float wantedY = wantedRotation.eulerAngles.y;
        while(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, wantedY)) > 5f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, wantedRotation, Time.deltaTime * 10f);
            yield return null;
        }

        footsteps.enabled = false;

        IsReadyToLoot = true;
    }
}
