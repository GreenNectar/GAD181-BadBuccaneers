using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointerController : MicroGamePlayerController
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private RectTransform pointPosition;

    RectTransform rectTransform;
    RectTransform parentRectTransform;

    protected override void Start()
    {
        base.Start();

        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(player.GetAxis("LeftMoveX"), player.GetAxis("LeftMoveY"), 0f);
        //float multiplier = ((float)Screen.width / (float)Screen.height);
        //float multiplier = Mathf.Sqrt((Screen.currentResolution.height * Screen.currentResolution.height) + (Screen.currentResolution.width * Screen.currentResolution.width)) / 2f;

        rectTransform.position += transform.TransformVector(movement * Time.deltaTime * speed);// * multiplier;

        ClampToWindow();

        if (player.GetButtonDown("Fire"))
        {
            IPlayerPointerHandler hoveredObject = GetHoveredObject();
            if (hoveredObject != null)
            {
                hoveredObject.Click(this);
            }
        }
    }

    private IPlayerPointerHandler GetHoveredObject()
    {
        RaycastHit[] hits = null;

        Canvas canvas = GetComponentInParent<Canvas>();
        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                Ray ray = Camera.main.ScreenPointToRay(pointPosition.position);
                hits = Physics.RaycastAll(ray);
                break;
            case RenderMode.ScreenSpaceCamera:
                hits = Physics.RaycastAll(Camera.main.transform.position, transform.position - Camera.main.transform.position);
                break;
            default:
                throw new System.Exception($"Canvas render mode '{canvas.renderMode}' is not currently supported");
        }

        if (hits.Length > 0)
        {
            IPlayerPointerHandler[] firstObjectWithClick = hits.OrderBy(r => r.distance).Select(r => r.collider.GetComponent<IPlayerPointerHandler>()).ToArray();
            if (firstObjectWithClick.Length > 0)
            {
                return firstObjectWithClick[0];
            }
        }

        return null;
    }

    void ClampToWindow()
    {
        Vector3 pos = rectTransform.localPosition;

        Vector3 minPosition = parentRectTransform.rect.min - rectTransform.rect.min;
        Vector3 maxPosition = parentRectTransform.rect.max - rectTransform.rect.max;

        pos.x = Mathf.Clamp(rectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(rectTransform.localPosition.y, minPosition.y, maxPosition.y);

        rectTransform.localPosition = pos;
    }
}
