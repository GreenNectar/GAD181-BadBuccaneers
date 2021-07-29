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

    public float holdingWeight;


    public Transform BombPosition => bombPosition;


    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();

        armsRig.weight = holdingWeight;
        Animator.SetLayerWeight(1, holdingWeight);
        Animator.speed = 1f + (holdingWeight * extraSpeedWhenHolding);
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.gameObject.GetComponent<HotBombPlayerController>())
    //    {
    //        if (HotBombController.)
    //        //hit.gameObject.GetComponent<HotBombPlayerController>();
    //    }
    //}


}
