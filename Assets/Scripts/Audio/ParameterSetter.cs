using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterSetter : MonoBehaviour
{
    [SerializeField, ParamRef]
    private string parameter;

    public void SetParameter(float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(parameter, value);
    }
}
