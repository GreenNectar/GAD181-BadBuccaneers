using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float maxDistance = 25f;

    public int points = 1;


    private float distance = 0f;

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime * speed;
        distance += delta;
        transform.position += transform.forward * delta;

        if (distance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
