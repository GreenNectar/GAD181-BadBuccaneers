using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterFMODEvents", menuName = "Bad Buccaneers/Character FMOD Events")]
[Serializable]
public class CharacterFMODEvents : ScriptableObject
{
    [SerializeField, EventRef]
    private string death;
    [SerializeField, EventRef]
    private string drowning;
    [SerializeField, EventRef]
    private string happy;
    [SerializeField, EventRef]
    private string jump;
    [SerializeField, EventRef]
    private string sad;
    [SerializeField, EventRef]
    private string startled;

    public void Death(Vector3 position)
    {
        RuntimeManager.PlayOneShot(death, position);
    }

    public void Drowning(Vector3 position)
    {
        RuntimeManager.PlayOneShot(drowning, position);
    }

    public void Happy(Vector3 position)
    {
        RuntimeManager.PlayOneShot(happy, position);
    }

    public void Jump(Vector3 position)
    {
        RuntimeManager.PlayOneShot(jump, position);
    }

    public void Sad(Vector3 position)
    {
        RuntimeManager.PlayOneShot(sad, position);
    }

    public void Startled(Vector3 position)
    {
        RuntimeManager.PlayOneShot(startled, position);
    }





    public void Death(GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(death, gameObject);
    }

    public void Drowning(GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(drowning, gameObject);
    }

    public void Happy(GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(happy, gameObject);
    }

    public void Jump(GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(jump, gameObject);
    }

    public void Sad(GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(sad, gameObject);
    }

    public void Startled(GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(startled, gameObject);
    }
}
