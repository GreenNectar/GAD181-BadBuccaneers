using FMOD.Studio;
using FMODUnity;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private CameraCanvasPosition[] positions;

    [SerializeField]
    private CameraCanvasPosition starting;

    [SerializeField, EventRef]
    private string moveToEvent;
    [SerializeField, EventRef]
    private string goBackEvent;
    [SerializeField, EventRef]
    private string menuEvent;
    [SerializeField, ParamRef]
    private string ambientParameter;
    private EventInstance menuEventInstance;





    private CameraCanvasPosition current;
    private CameraCanvasPosition previous;

    [Serializable]
    private class CameraCanvasPosition
    {
        public string name;
        public Transform transform;
        public CanvasGroup canvasGroup;
        public Selectable firstSelected;
    }

    private bool isMoving = false;
    private bool canGoBack = true;

    private GameObject selectedObject;
    public Registry StopBackRegister = new Registry();

    private Stack<CameraCanvasPosition> undo = new Stack<CameraCanvasPosition>();

    private void OnEnable()
    {
        StopBackRegister.onOccupied.AddListener(() => canGoBack = false);
        StopBackRegister.onUnOccupied.AddListener(() => canGoBack = true);
        menuEventInstance = RuntimeManager.CreateInstance(menuEvent);
        menuEventInstance.start();
    }

    private void OnDisable()
    {
        StopBackRegister.onOccupied.RemoveAllListeners();
        canGoBack = true;
        menuEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        menuEventInstance.release();
    }

    private void Start()
    {
        current = starting;

        // Haha ccp... Pooh Bear
        foreach (var ccp in positions)
        {
            if (ccp != current)
            {
                ccp.canvasGroup.alpha = 0f;
                ccp.canvasGroup.gameObject.SetActive(false);
            }
        }

        if (PlayerManager.PlayerCount > 0)
        {
            MoveTo(positions[0].name);
        }
    }

    //int cur = 0;
    private void Update()
    {
        if (!isMoving)
        {
            // Add the first player to the game and move to the first menu screen
            if (PlayerManager.PlayerCount == 0)
            {
                foreach (var player in ReInput.players.AllPlayers)
                {
                    if (player.GetButtonDown("Start"))
                    {
                        RuntimeManager.StudioSystem.setParameterByName(ambientParameter, 1f); // Play ambient and music
                        PlayerManager.AddPlayer(0, player.id);
                        MoveTo(positions[0].name);
                    }
                }
            }
            // Go back...
            else
            {
                if (PlayerManager.GetPlayer(0).GetButtonDown("Cancel") && canGoBack)
                {
                    GoBack();
                }
            }
        }

        // Hard setting of the position as the transform might be moving so we want to constantly follow it
        if (!isMoving)
        {
            camera.transform.position = current.transform.position;
            camera.transform.rotation = current.transform.rotation;
        }

        //! This bit stops the mouse from being able to deselect the current selected object
        // If we have a selected object, set it
        if (EventSystem.current.currentSelectedGameObject)
        {
            selectedObject = EventSystem.current.currentSelectedGameObject;
        }
        // Otherwise reselect the previous selected object
        else
        {
            EventSystem.current.SetSelectedGameObject(selectedObject);
        }
    }

    /// <summary>
    /// Move to the screen with the given name
    /// </summary>
    /// <param name="name"></param>
    public void MoveTo(string name)
    {
        previous = current;
        undo.Push(previous);
        current = positions.First(p => p.name == name);

        // Play the move to sfx
        RuntimeManager.PlayOneShot(moveToEvent);

        StartCoroutine(MoveToRoutine(previous, current));
    }

    private IEnumerator MoveToRoutine(CameraCanvasPosition from, CameraCanvasPosition to)
    {
        isMoving = true;

        // Deselect the current selected object
        EventSystem.current.SetSelectedGameObject(EventSystem.current.gameObject);

        to.canvasGroup.gameObject.SetActive(true);

        if (to.canvasGroup.GetComponent<StartMenuEvents>())
            to.canvasGroup.GetComponent<StartMenuEvents>().Enter();
        if (from.canvasGroup.GetComponent<StartMenuEvents>())
            from.canvasGroup.GetComponent<StartMenuEvents>().Exit();

        float time = 0f;
        float timeBeforeAlpha = 0.5f;
        float alphaMultiplier = 1f / (1f - timeBeforeAlpha);
        while(time < 1f)
        {
            time += Time.deltaTime;
            time = Mathf.Clamp(time, 0f, 1f);
            from.canvasGroup.alpha = Mathf.Clamp(((1f - time) - timeBeforeAlpha) * alphaMultiplier, 0f, 1f);
            to.canvasGroup.alpha = Mathf.Clamp((time - timeBeforeAlpha) * alphaMultiplier, 0f, 1f);
            camera.transform.position = Mathfx.Hermite(from.transform.position, to.transform.position, time);
            camera.transform.rotation = Mathfx.Hermite(from.transform.rotation, to.transform.rotation, time);
            yield return null;
        }

        from.canvasGroup.gameObject.SetActive(false);

        // Select the current canvas's first selected object (if null it deselects it)
        EventSystem.current.SetSelectedGameObject(current.firstSelected ? current.firstSelected.gameObject : EventSystem.current.gameObject);

        isMoving = false;
    }

    /// <summary>
    /// Go back to the previous window
    /// </summary>
    public void GoBack()
    {
        previous = current;
        current = undo.Pop();

        // Play go back sfx
        RuntimeManager.PlayOneShot(goBackEvent);

        // If we made it back to the title menu
        if (undo.Count == 0)
        {
            RuntimeManager.StudioSystem.setParameterByName(ambientParameter, 0f); // Play the ambient only
            PlayerManager.RemovePlayer(0);
        }

        StartCoroutine(MoveToRoutine(previous, current));
    }
}
