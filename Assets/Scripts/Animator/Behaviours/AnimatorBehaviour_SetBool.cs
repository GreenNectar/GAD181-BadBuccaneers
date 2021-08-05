using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBehaviour_SetBool : AnimatorBehaviour_PerformOnState
{
    public bool value;

    public override void OnPerformed(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(parameterName, value);
    }
}
