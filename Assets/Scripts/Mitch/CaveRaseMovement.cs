using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CaveRaseMovement : MicroGamePlayerController
{
    public float fowardSpeed = 1f;
    public float moveSpeed = 1f;
    public Transform jumpSpot;
    public Transform duckSpot;
    public Transform defultSpot;

    [SerializeField]
    private bool isMoving;
    private Vector3 desiredPos;
   
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * fowardSpeed);

        if(!isMoving && player.GetButtonDown("Jump"))
        {
            //desiredPos = jumpSpot.position;
            desiredPos = new Vector3(transform.position.x, transform.position.y, jumpSpot.position.z);
            isMoving = true;
        }

        if(!isMoving && player.GetButtonDown("Duck"))
        {
            desiredPos = duckSpot.position;
            isMoving = true;
        }

        if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredPos, moveSpeed * Time.deltaTime);

            if(transform.position == desiredPos)
            {
                isMoving = false;
                transform.position = desiredPos;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, defultSpot.position, moveSpeed * Time.deltaTime);
        }
    }
}
