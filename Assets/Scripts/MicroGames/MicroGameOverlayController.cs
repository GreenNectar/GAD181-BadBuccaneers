using FMOD.Studio;
using FMODUnity;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicroGameOverlayController : MonoBehaviour
{
    [SerializeField]
    private RenderTexture cameraRenderTexture;

    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private ControlPanel controlPanel;
    [SerializeField]
    private Transform controlsParent;
    [SerializeField]
    private TextMeshProUGUI developers;

    [SerializeField]
    private GameObject[] readyObjects;

    private bool[] isReady;

    private EventInstance voiceOverInstance;

    // Start is called before the first frame update
    void Start()
    {
        SetCamera();
        InitialiseUI();

        isReady = new bool[PlayerManager.PlayerCount];
        voiceOverInstance = RuntimeManager.CreateInstance(GameManager.Instance.currentMicroGame.voiceOverEvent);
        Invoke("StartVoiceOver", 1.5f); // Delay the vo
    }

    private void StartVoiceOver()
    {
        voiceOverInstance.start();
    }

    private void OnDisable()
    {
        // Fade out the vo when we close the overlay
        voiceOverInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        voiceOverInstance.release();
    }

    private void Update()
    {
        for (int i = 0; i < PlayerManager.PlayerCount; i++)
        {
            if (!isReady[i] && PlayerManager.GetPlayer(i).GetButtonDown("Ready"))
            {
                isReady[i] = true;
                readyObjects[i].SetActive(true);
                CheckReady();
            }
        }
    }

    /// <summary>
    /// If everyone is ready it loads out of practice
    /// </summary>
    private void CheckReady()
    {
        bool allReady = !isReady.Contains(false);
        if (allReady)
        {
            GameManager.Instance.LoadOutOfPractice();
        }
    }

    /// <summary>
    /// Sets all the text to the current microgame
    /// </summary>
    private void InitialiseUI()
    {
        GenerateControls();
        title.text = GameManager.Instance.currentMicroGame.title;
        description.text = GameManager.Instance.currentMicroGame.description;
        developers.text = "Developers - " + GameManager.Instance.currentMicroGame.developers;
    }

    /// <summary>
    /// Generates all the text objects for each control set by the scriptable object
    /// </summary>
    private void GenerateControls()
    {
        foreach (var control in GameManager.Instance.currentMicroGame.controls)
        {
            ControlPanel panel = Instantiate(controlPanel, controlsParent);
            panel.buttons.GetComponent<ControllerTextReplacerAllPlayers>().SetStarting(control.buttons);
            panel.buttons.text = control.buttons;
            panel.description.text = control.description;
        }
    }

    public void SetCamera()
    {
        StartCoroutine(SetCameraSequence());
    }

    /// <summary>
    /// Sets the render texture to the camera/s in the scene
    /// </summary>
    /// <returns></returns>
    IEnumerator SetCameraSequence()
    {
        Debug.LogWarning("Trying to set camera");

        while (!FindObjectOfType<CameraController>())
        {
            Debug.LogWarning("Cannot find camera controller in scene");
            yield return null;
        }

        CameraController cameraController = FindObjectOfType<CameraController>();
        foreach (var camera in cameraController.Cameras)
        {
            camera.targetTexture = cameraRenderTexture;
        }
    }
}