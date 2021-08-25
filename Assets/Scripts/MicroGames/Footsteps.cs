using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField, EventRef]
    private string walkEvent;
    private float walkDistance;
    [SerializeField]
    private float maxWalk = 1f;

    Vector3 previousPosition;

    private void OnEnable()
    {
        previousPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Play the walk sound
        walkDistance += (transform.localPosition - previousPosition).magnitude;
        previousPosition = transform.localPosition;
        if (walkDistance > maxWalk)
        {
            walkDistance %= maxWalk;
            RuntimeManager.PlayOneShotAttached(walkEvent, gameObject);
        }
    }
}
