using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBehaviour_SetRandomInt : AnimatorBehaviour_PerformOnState
{
    public int min;
    public int max;

    public override void OnPerformed(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(parameterName, Random.Range(min, max));
    }
}
