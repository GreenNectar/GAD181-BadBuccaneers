using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public enum SplitScreenStyle
    {
        OneScreen,
        Vertical,
        Horizontal,
        OneTopThreeBottom,
        FourScreen,
    }

    [SerializeField]
    private SplitScreenStyle splitScreenStyle = SplitScreenStyle.FourScreen;

    // One Screen
    [SerializeField, BoxGroup("Cameras"), ShowIf("IsOneScreen"), Tooltip("Honestly bro, you don't even need this in if it's one-screen")]
    private Camera oneScreenCamera;

    // Top/Bottom, Left/Right
    [SerializeField, BoxGroup("Cameras"), ShowIf(EConditionOperator.Or, "IsTopBottom", "IsLeftRight")]
    private Camera cameraTopLeft;
    [SerializeField, BoxGroup("Cameras"), ShowIf(EConditionOperator.Or, "IsTopBottom", "IsLeftRight")]
    private Camera cameraBottomRight;

    //TODO Add game manager that controls whose turn it is as the top player, right now it is just the first player
    // Four Screen, OneTopThreeBottom
    [SerializeField, BoxGroup("Cameras"), ShowIf(EConditionOperator.Or, "IsFourScreen", "IsOneTopThreeBottom"), Tooltip("Will be top camera if selected 'OneTopThreeBottom'")]
    private Camera player1Camera;
    [SerializeField, BoxGroup("Cameras"), ShowIf(EConditionOperator.Or, "IsFourScreen", "IsOneTopThreeBottom"), Tooltip("Will be a bottom camera if selected 'OneTopThreeBottom'")]
    private Camera player2Camera;
    [SerializeField, BoxGroup("Cameras"), ShowIf(EConditionOperator.Or, "IsFourScreen", "IsOneTopThreeBottom"), Tooltip("Will be a bottom camera if selected 'OneTopThreeBottom'")]
    private Camera player3Camera;
    [SerializeField, BoxGroup("Cameras"), ShowIf(EConditionOperator.Or, "IsFourScreen", "IsOneTopThreeBottom"), Tooltip("Will be a bottom camera if selected 'OneTopThreeBottom'")]
    private Camera player4Camera;
    [SerializeField, BoxGroup("Cameras"), ShowIf("IsFourScreen"), Tooltip("Will be the fourth screen when there is three players")]
    private Camera spectatorCamera;
    [SerializeField, BoxGroup("Cameras"), ShowIf("IsFourScreen"), Tooltip("Determines whether or no the camera is vertical split-screen when there's two players\n(True - Vertical, False - Horizontal)")]
    private bool isTwoPlayerVertical = true;

    // These are used for the ShowIf attributes
    private bool IsOneScreen => splitScreenStyle == SplitScreenStyle.OneScreen;
    private bool IsTopBottom => splitScreenStyle == SplitScreenStyle.Vertical;
    private bool IsLeftRight => splitScreenStyle == SplitScreenStyle.Horizontal;
    private bool IsOneTopThreeBottom => splitScreenStyle == SplitScreenStyle.OneTopThreeBottom;
    private bool IsFourScreen => splitScreenStyle == SplitScreenStyle.FourScreen;

    [SerializeField, Range(1, 4)]
    private int playersToVisualise = 1;


    private Camera[] cameras;
    public Camera[] Cameras
    {
        get
        {
            if (cameras == null)
            {
                var tempCameras = new List<Camera>();
                if (player1Camera != null) tempCameras.Add(player1Camera);
                if (player2Camera != null) tempCameras.Add(player2Camera);
                if (player3Camera != null) tempCameras.Add(player3Camera);
                if (player4Camera != null) tempCameras.Add(player4Camera);
                if (spectatorCamera != null) tempCameras.Add(spectatorCamera);
                if (cameraTopLeft != null) tempCameras.Add(cameraTopLeft);
                if (cameraBottomRight != null) tempCameras.Add(cameraBottomRight);
                if (oneScreenCamera != null) tempCameras.Add(oneScreenCamera);
                if (spectatorCamera != null) tempCameras.Add(spectatorCamera);
                cameras = tempCameras.ToArray();
            }
            return cameras;
        }
    }

#if UNITY_EDITOR
    // A button that allows us to visualise the amount of players, this is so we can check if we have the cameras correct
    [Button]
    private void Visualise()
    {
        RefreshCamera(playersToVisualise);
    }
#endif

    private void Start()
    {
        int playerCount = PlayerManager.PlayerCount > 0 ? PlayerManager.PlayerCount : 4; // This is so we can see all the cameras when testing without having players added to the manager
        RefreshCamera(playerCount);
    }

    private void RefreshCamera(int players = 0)//bool isTesting = false)
    {
        // Enable all cameras
        ToggleCameras(true, player1Camera, player2Camera, player3Camera, player4Camera, oneScreenCamera);

        // Go through and enable the correct cameras, while also adjusting their positions and scales on-screen
        switch (splitScreenStyle)
        {
            case SplitScreenStyle.OneScreen:
                oneScreenCamera.rect = new Rect(0f, 0f, 1f, 1f); // It be the one and only screen, my young Padawan
                // Enable just the onescreen camera
                ToggleCameras(true, cameraTopLeft);
                ToggleCameras(false, player1Camera, player2Camera, player3Camera, player4Camera);
                break;
            case SplitScreenStyle.Vertical:
                SetVertical(cameraTopLeft, cameraBottomRight); // Vertical babbbyyy
                ToggleCameras(false, player3Camera, player4Camera);
                break;
            case SplitScreenStyle.Horizontal:
                SetHorizonal(cameraTopLeft, cameraBottomRight); // Horizontal my duuuuude
                ToggleCameras(false, player3Camera, player4Camera);
                break;
            case SplitScreenStyle.OneTopThreeBottom:
                SetOneTopThreeBottom(players); // It does the thing it says, and more!
                break;
            case SplitScreenStyle.FourScreen:
                SetFourScreens(players); // I made a function, you proud of me daddy ;-;
                break;
            default:
                break;
        }
    }

    private void SetOneTopThreeBottom(int players)
    {
        if (players == 1) // One player, should not be an option!!!
        {
            player1Camera.rect = new Rect(0f, 0f, 1f, 1f);
            ToggleCameras(false, player2Camera, player3Camera, player4Camera);
            Debug.LogError("This should not be an option, figure out why this is able to happen!");
        }
        else if (players == 2) // Two player
        {
            SetVertical(player1Camera, player2Camera);
            ToggleCameras(false, player3Camera, player4Camera);
        }
        else if (players == 3) // Three Player
        {
            SetVertical(player1Camera, player2Camera, player3Camera);
            ToggleCameras(false, player4Camera);
        }
        else if (players == 4) // Four Player
        {
            SetVertical(player1Camera, player2Camera, player3Camera, player4Camera);
        }
    }

    private void SetFourScreens(int players)
    {
        if (players == 1) // One player (this is for practice compatibility)
        {
            player1Camera.rect = new Rect(0f, 0f, 1f, 1f); // One Screen
            ToggleCameras(false, player2Camera, player3Camera, player4Camera);
        }
        else if (players == 2) // Two player
        {
            if (isTwoPlayerVertical)
            {
                SetVertical(player1Camera, player2Camera);
            }
            else
            {
                SetHorizonal(player1Camera, player2Camera);
            }
            ToggleCameras(false, player3Camera, player4Camera);
        }
        else if (players >= 3) // Three or more
        {
            player1Camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f); // Top Left
            player2Camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f); // Top Right
            player3Camera.rect = new Rect(0f, 0f, 0.5f, 0.5f); // Bottom Left
            player4Camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f); // Bottom Right

            // If we have three players we want to have the fourth camera as the spectator camera
            if (players == 3)
            {
                ToggleCameras(false, player4Camera);
                ToggleCameras(true, spectatorCamera);
                CameraRect(spectatorCamera, new Rect(0.5f, 0f, 0.5f, 0.5f));
            }
            else
            {
                ToggleCameras(false, spectatorCamera);
            }
        }
    }

    /// <summary>
    /// Set a vertical layout of two to four cameras
    /// </summary>
    /// <param name="topCamera">Camera to be rendered at the top half</param>
    /// <param name="bottomCameras">These cameras will be stretched evenly in the bottom half of the screen</param>
    private void SetVertical(Camera topCamera, params Camera[] bottomCameras)
    {
        topCamera.rect = new Rect(0f, 0.5f, 1f, 0.5f); // Top

        int cameras = bottomCameras.Length; // Number of cameras

        // Scale all the bottom cameras based on the number of cameras added
        for (int i = 0; i < cameras; i++)
        {
            bottomCameras[i].rect = new Rect((float)i / cameras, 0f, 1f / cameras, 0.5f); // Bottom cameras
        }
    }

    /// <summary>
    /// Set a horizontal layout of two cameras
    /// </summary>
    /// <param name="cameraLeft">Camera to render on the left half</param>
    /// <param name="cameraRight">Camera to render on the right half</param>
    private void SetHorizonal(Camera cameraLeft, Camera cameraRight)
    {
        cameraLeft.rect = new Rect(0f, 0f, 0.5f, 1f); // Left
        cameraRight.rect = new Rect(0.5f, 0f, 0.5f, 1f); // Right
    }

    /// <summary>
    /// Toggles the cameras 'enabled' field
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="cameras"></param>
    private void ToggleCameras(bool enable, params Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            if (camera != null)
            {
                camera.enabled = enable;
            }
        }
    }

    /// <summary>
    /// Change the camera's rect
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="rect"></param>
    private void CameraRect(Camera camera, Rect rect)
    {
        if (camera != null)
        {
            camera.rect = new Rect(rect); // Copy the rect (not sure if needed)
        }
    }
}