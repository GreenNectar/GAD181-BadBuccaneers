using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HotBombPlayerController : TopDownPlayerController
{
    [SerializeField]
    private Transform bombPosition;

    [SerializeField]
    private Transform leftHandPosition;
    [SerializeField]
    private Transform rightHandPosition;

    [SerializeField]
    private Rig armsRig;

    [SerializeField]
    private float extraSpeedWhenHolding;
    [SerializeField]
    private float extraLerpSpeedWhenHolding;

    [SerializeField, Tooltip("This is the model attached to the prefab/it is the skin")]
    private GameObject[] playerModels;
    [SerializeField, Tooltip("This is the skeleton model attached to the prefab/it is the skin")]
    private GameObject skeletonModel;

    [SerializeField]
    private Footsteps footsteps;

    private float tempLerpSpeed;
    public float holdingWeight;
    public Transform BombPosition => bombPosition;

    protected override void Start()
    {
        base.Start();
        tempLerpSpeed = lerpSpeed;
    }

    protected override void Update()
    {
        base.Update();

        SetHolding();
    }

    public void Kill()
    {
        // Stop holding the bomb
        holdingWeight = 0f;
        SetHolding();

        // Look towards the camera
        transform.eulerAngles = new Vector3(0f, Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up).eulerAngles.y, 0f);
        // Play the end animation
        Animator.SetTrigger("End");
        // Set the speed to 1 so we don't have weird animations
        Animator.speed = 1f;

        // Change the player model to the skeleton one
        foreach (var model in playerModels)
        {
            model.SetActive(false);
        }
        skeletonModel.SetActive(true);

        // Stop playing footstep sounds
        footsteps.enabled = false;

        // Stop vibing
        Vibrator.Instance.Vibrate(PlayerNumber, 0, 0f);
        Vibrator.Instance.Vibrate(PlayerNumber, 1, 0f);

        // Play the death sound for the player
        PlayerManager.GetPlayerFMODEvent(PlayerNumber).Death(gameObject);

        ScoreManager.Instance.EndPlayer(PlayerNumber);

        // Disable this player controller component
        enabled = false;
    }

    public void SetHolding()
    {
        armsRig.weight = holdingWeight;
        Animator.SetLayerWeight(1, holdingWeight);
        Animator.speed = animatorSpeed + (extraSpeedWhenHolding * holdingWeight);
        lerpSpeed = tempLerpSpeed + (extraLerpSpeedWhenHolding * holdingWeight);
    }
}
