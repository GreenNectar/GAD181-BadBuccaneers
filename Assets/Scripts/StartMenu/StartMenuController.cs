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
    private Camera camera;

    [SerializeField]
    private CameraCanvasPosition[] positions;

    [SerializeField]
    private CameraCanvasPosition starting;

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

    private Stack<CameraCanvasPosition> undo = new Stack<CameraCanvasPosition>();

    private bool isMoving = false;

    private GameObject selectedObject;

    private void Start()
    {
        current = starting;

        foreach (var ccp in positions)
        {
            if (ccp != current)
            {
                ccp.canvasGroup.gameObject.SetActive(false);
                ccp.canvasGroup.alpha = 0f;
            }
        }
    }

    //int cur = 0;
    private void Update()
    {
        if (!isMoving)
        {
            //foreach (var player in ReInput.players.AllPlayers)
            //{
            //    if (player.GetButtonDown("Start"))
            //    {
            //        if (PlayerManager.PlayerCount == 0) PlayerManager.AddPlayer(0, player.id);
            //        MoveTo(positions[cur].name);
            //        cur++;
            //    }
            //}
            //{
            //    if (PlayerManager.GetPlayer(0).GetButtonDown("Cancel"))
            //    {
            //        cur--;
            //        GoBack();
            //    }
            //}

            if (PlayerManager.PlayerCount == 0)
            {
                foreach (var player in ReInput.players.AllPlayers)
                {
                    if (player.GetButtonDown("Start"))
                    {
                        PlayerManager.AddPlayer(0, player.id);
                        MoveTo(positions[0].name);
                    }
                }
            }
            else
            {
                if (PlayerManager.GetPlayer(0).GetButtonDown("Cancel"))
                {
                    GoBack();
                }
            }
        }

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

    public void MoveTo(string name)
    {
        previous = current;
        undo.Push(previous);
        current = positions.First(p => p.name == name);

        StartCoroutine(MoveToRoutine(previous, current));
    }

    private IEnumerator MoveToRoutine(CameraCanvasPosition from, CameraCanvasPosition to)
    {
        isMoving = true;

        // Deselect the current selected object
        EventSystem.current.SetSelectedGameObject(EventSystem.current.gameObject);

        to.canvasGroup.gameObject.SetActive(true);

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

    public void GoBack()
    {
        previous = current;
        current = undo.Pop();

        if (undo.Count == 0)
        {
            PlayerManager.RemovePlayer(0);
        }

        StartCoroutine(MoveToRoutine(previous, current));
    }
}
