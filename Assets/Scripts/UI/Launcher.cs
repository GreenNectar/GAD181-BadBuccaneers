using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    public CanvasGroup canvasGroup;

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled && canvasGroup.alpha == 1 && PlayerManager.GetPlayer(0).GetButtonDown("Submit"))
        {
            GameManager.Instance.GenerateRandomLevels();
            GameManager.Instance.LoadNextLevel();
        }
    }
}
