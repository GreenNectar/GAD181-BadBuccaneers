using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
    [SerializeField, Scene]
    private string[] sceneNames;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadScene(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            LoadScene(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            LoadScene(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LoadScene(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            LoadScene(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            LoadScene(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            LoadScene(9);
        }
    }

    private void LoadScene(int sceneNumber)
    {
        if (sceneNumber < sceneNames.Length)
        {
            PlayerManager.ShiftPlayers();
            SceneManager.LoadScene(sceneNames[sceneNumber]);
        }
    }
}
