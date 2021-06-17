using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public bool doSwitch;
    public bool autoAdjust = false;
    public PlayerInputManager playerInputManager;

    public void OnPlayerJoined()
    {
        RefreshCamera();
    }

    public void OnPlayerLeft()
    {
        RefreshCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (doSwitch)
        {
            doSwitch = false;

            playerInputManager.splitScreen = !playerInputManager.splitScreen;

            FindObjectsOfType<Camera>(true).First(c => c.tag == "MainCamera").enabled = !playerInputManager.splitScreen;
            foreach (var camera in FindObjectsOfType<Camera>(true).Where(c => c.tag != "MainCamera"))
            {
                camera.enabled = playerInputManager.splitScreen;
            }
        }
    }

    void RefreshCamera()
    {
        if (!autoAdjust) return;

        Camera[] cameras = FindObjectsOfType<PlayerInput>(true).Select(p => p.GetComponentInChildren<Camera>()).ToArray();
        int count = cameras.Length;

        foreach (var camera in cameras)
        {
            if (count == 2)
            {
                Rect rect = playerInputManager.splitScreenArea;
                Rect cameraRect = camera.rect;
                cameraRect.y = rect.height / 4f;
                camera.rect = cameraRect;
            }
            else
            {

            }
        }
    }
}