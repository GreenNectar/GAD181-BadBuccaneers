using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelGamePlayerController : TopDownPlayerController
{
    [SerializeField]
    private Transform wipeOutPosition;

    [SerializeField]
    private GameObject impactFX;

    private bool hasWipedOut = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!hasWipedOut)
            base.Update();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Trip"))
        {
            Animator.SetTrigger("WipeOut");
            hasWipedOut = true;
            StartCoroutine(MoveToOutside());
            Instantiate(impactFX, transform.TransformPoint(GetComponent<CharacterController>().center), Quaternion.identity);
        }
    }

    private IEnumerator MoveToOutside()
    {
        float time = 0f;
        Vector3 startingPosition = transform.position;
        GetComponent<CharacterController>().enabled = false;
        while (time < 1f)
        {
            time += Time.deltaTime;
            transform.position = Mathfx.Sinerp(startingPosition, wipeOutPosition.position, time);
            yield return null;
        }
    }
}
