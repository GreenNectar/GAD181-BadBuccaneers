using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is to be able to set values in an animator using an animator
/// </summary>
public class AnimatorSetter : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private string parameterName;
    private int layerIndex;

    /// <summary>
    /// Sets the animator it will use to set the parameters
    /// </summary>
    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }

    /// <summary>
    /// Sets the name of the object it will use when setting the values
    /// </summary>
    /// <param name="name"></param>
    public void SetString(string name)
    {
        parameterName = name;
    }

    /// <summary>
    /// Send float values to the Animator to affect transitions
    /// </summary>
    /// <param name="value"></param>
    public void SetFloat(float value)
    {
        animator.SetFloat(parameterName, value);
    }

    public void SetLayerIndex(int index)
    {
        layerIndex = index;
    }

    public void SetLayerWeight(float weight)
    {
        animator.SetLayerWeight(layerIndex, weight);
    }
}
