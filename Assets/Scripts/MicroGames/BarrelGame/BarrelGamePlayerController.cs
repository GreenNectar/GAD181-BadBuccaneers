using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelGamePlayerController : TopDownPlayerController
{
    [SerializeField]
    private Transform wipeOutPosition;

    [SerializeField]
    private GameObject impactFX;

    [SerializeField, EventRef]
    private string impactEvent;

    public bool hasWipedOut { get; private set; } = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!hasWipedOut)
            base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trip"))
        {
            WipeOut();
        }
    }

    public void WipeOut()
    {
        Animator.SetTrigger("WipeOut");
        hasWipedOut = true;
        StartCoroutine(MoveToOutside());
        Instantiate(impactFX, transform.TransformPoint(GetComponent<CharacterController>().center), Quaternion.identity);
        ScoreManager.Instance.EndPlayer(PlayerNumber);
        Vibrator.Instance.ImpactVbration(PlayerNumber, 0, 0.5f);
        Vibrator.Instance.ImpactVbration(PlayerNumber, 1, 0.5f);
        PlayerManager.GetPlayerFMODEvent(PlayerNumber).Death(gameObject);
        RuntimeManager.PlayOneShot(impactEvent, transform.position);
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
