using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    public float speed = 1f;
    [SerializeField]
    private float heightLerpSpeed = 10f;
    [SerializeField]
    private float maxDistance = 30f;
    [SerializeField]
    private float intialOffset = -5f;
    [SerializeField]
    private float readyUpDistance = 2f;

    [SerializeField]
    private CapsuleCollider capsuleCollider;
    [SerializeField]
    private Transform barrelModel;

    private Vector3 startingPosition;
    private Vector3 direction;
    private float offset = 0f;
    private float circumference = 0f;
    private float currentHeight = 0f;
    private Vector3 position;
    private float offsetDelta = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        direction = transform.forward;
        circumference = 2f * Mathf.PI * capsuleCollider.radius;
        currentHeight = transform.position.y;
        offset = intialOffset;

        UpdateTransform();

        StartCoroutine(RollSequence());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        // Previous position
        if (Physics.Raycast((Vector3.up * 100f) + startingPosition + direction * offset, Vector3.down, out RaycastHit hit, 1000f, LayerMask.GetMask("Default")))
        {
            transform.position = hit.point + Vector3.up * capsuleCollider.radius;
        }

        // Lerp the height
        currentHeight = Mathf.Lerp(currentHeight, transform.position.y, Time.deltaTime * heightLerpSpeed * speed);
        position = transform.position;
        position.y = currentHeight;
        transform.position = position;


        // Rotate
        barrelModel.Rotate(Vector3.up, -360f * (offsetDelta / circumference), Space.Self);
    }

    float startingOffset = 0f;
    private IEnumerator RollSequence()
    {

        // Use offset to change position of barrel

        // Line up
        while (offset < 0f)
        {
            startingOffset = offset;
            offset += Time.deltaTime * speed;
            offset = Mathf.Clamp(offset, intialOffset, 0f);

            offsetDelta = (offset - startingOffset);

            yield return null;
        }

        // Ready up (go backwards a bit)
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * speed / 2f;
            if (time > 1f) time = 1f;
            startingOffset = offset;
            offset = -Mathf.SmoothStep(0f, readyUpDistance, time);

            offsetDelta = (offset - startingOffset);

            yield return null;
        }

        // Roll
        while (true)
        {
            offset += Time.deltaTime * speed;

            if (offset > maxDistance)
                Destroy(gameObject);

            offsetDelta = Time.deltaTime * speed;
            //barrelModel.Rotate(Vector3.up, -360f * ((Time.deltaTime * speed) / circumference), Space.Self);

            yield return null;
        }
    }
}
