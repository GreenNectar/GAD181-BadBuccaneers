using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicroGameOverlayController : MonoBehaviour
{
    //[SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private RenderTexture renderTexture;

    [SerializeField]
    private RawImage renderImage;

    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private ControlPanel controlPanel;
    [SerializeField]
    private Transform controlsParent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetCamera());
        InitialiseUI();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitialiseUI()
    {
        GenerateControls();
        title.text = GameManager.Instance.currentMicroGame.title;
        description.text = GameManager.Instance.currentMicroGame.description;
    }

    private void GenerateControls()
    {
        foreach (var control in GameManager.Instance.currentMicroGame.controls)
        {
            ControlPanel panel = Instantiate(controlPanel, controlsParent);
            panel.buttons.text = control.buttons;
            panel.description.text = control.description;
        }
    }

    IEnumerator SetCamera()
    {
        Debug.LogWarning("Trying to set camera");

        while (!FindObjectOfType<CameraController>())
        {
            Debug.LogWarning("Cannot find camera controller in scene");
            yield return null;
        }

        cameraController = FindObjectOfType<CameraController>();

        foreach (var camera in cameraController.Cameras)
        {
            Debug.LogWarning($"Setting render texture on {camera.name}");

            camera.targetTexture = renderTexture;
        }
    }
}
