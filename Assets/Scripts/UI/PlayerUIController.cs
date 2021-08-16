using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerUIController : MonoBehaviour, IMicroGameLoad
{
    [SerializeField]
    new private Camera camera;
    [SerializeField]
    private UniversalAdditionalCameraData additionalCameraData;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RenderTexture renderTexture;

    [SerializeField]
    private GameObject cornerPanels;
    [SerializeField]
    private GameObject bottomPanels;
    [SerializeField]
    private GameObject LeftPanels;

    public void OnMicroGameLoad()
    {
        Setup();
    }

    private void Setup()
    {
        // This is just to get the rendering correct. Kind of annoying I have to do this :/
        if (GameManager.Instance.IsInPracticeMode)
        {
            // The default settings to allow it to render in the micro game overlay
            additionalCameraData.renderType = CameraRenderType.Base;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            camera.targetTexture = renderTexture;
        }
        else
        {
            // Without this the camera renders black overtop
            additionalCameraData.renderType = CameraRenderType.Overlay;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            camera.targetTexture = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        cornerPanels.SetActive(false);
        bottomPanels.SetActive(false);
        LeftPanels.SetActive(false);

        if (GameManager.Instance.currentMicroGame != null)
        {
            switch (GameManager.Instance.currentMicroGame.scoreLayout)
            {
                case MicroGame.ScoreLayout.Corners:
                    cornerPanels.SetActive(true);
                    break;
                case MicroGame.ScoreLayout.Bottom:
                    bottomPanels.SetActive(true);
                    break;
                case MicroGame.ScoreLayout.Left:
                    LeftPanels.SetActive(true);
                    break;
                default:
                    cornerPanels.SetActive(true);
                    break;
            }
        }
        else
        {
            bottomPanels.SetActive(true);

            Debug.LogError("Micro game was not initialised before this scene was loaded, this should only occur in the editor!");
            //throw new System.Exception("Micro game was not initialised before this scene was loaded, this should only occur in the editor!");
        }
    }
}
