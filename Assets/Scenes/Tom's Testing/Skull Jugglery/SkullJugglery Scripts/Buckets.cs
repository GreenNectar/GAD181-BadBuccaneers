using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buckets : PlayerPanelController
{

    //GameObject Score;
    //GameObject Canvas;

    //Canvas.GetComponent<SkullScoreManager>();


    //private object Start()
    //{
      
    //    GameObject.Find("Score").GetComponent<SkullScoreManager>().points;
    //}

    ////Score.GetComponent<SkullScoreManager>();


    //public UnityEvent unityEvent;

    public int Points = 1;



    void OnCollisionEnter(Collision collision)
    {   


        Destroy(collision.collider.gameObject);
    }
}
