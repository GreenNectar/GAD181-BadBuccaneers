using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buckets : PlayerPanelController
{
    public Vector3 startPosition;
    public float magnitude;

    //public int Points = 1;

    //GameObject Score; 
    //GameObject Canvas; 

    //Canvas.GetComponent<SkullScoreManager>(); 


    //private object Start() 
    //{ 

    //    GameObject.Find("Score").GetComponent<SkullScoreManager>().points; 
    //} 

    ////Score.GetComponent<SkullScoreManager>(); 


    //public UnityEvent unityEvent; 

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition + new Vector3(Mathf.Sin(Time.time), 0f, 0f) * magnitude;
    }


    //void OnCollisionEnter(Collision collision)
    //{


    //    Destroy(collision.collider.gameObject);
    //}
}
