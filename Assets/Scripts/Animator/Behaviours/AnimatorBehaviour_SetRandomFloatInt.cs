using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBehaviour_SetRandomFloatInt : AnimatorBehaviour_PerformOnState
{
    public int min;
    public int max;

    public override void OnPerformed(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(parameterName, Random.Range(min, max));
    }
}
