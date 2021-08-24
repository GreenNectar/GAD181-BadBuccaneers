using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Follow3D : MonoBehaviour
{
    public Transform target;

    public Vector3 worldOffset;

    public Vector3 screenOffset;

    RectTransform rect;
    //CanvasScaler canvasScaler;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        //canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 offset = screenOffset;
        //screenOffset.x *= canvasScaler.referenceResolution.x / Screen.width;
        //screenOffset.y *= canvasScaler.referenceResolution.y / Screen.height;

        rect.position = Camera.main.WorldToScreenPoint(target.position + worldOffset) + screenOffset;
    }
}
