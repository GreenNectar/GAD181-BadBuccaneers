using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField, Scene]
    private string resultScreen;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadResultsScreen()
    {
        SceneManager.LoadScene(resultScreen);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //// Call 'OnMicroGameLoad' on all objects that implement the interface
        //foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        //{
        //    if (obj is IMicroGameLoad)
        //    {
        //        (obj as IMicroGameLoad).OnMicroGameLoad();
        //    }
        //}

        // Does the same thing as above, just makes more sense to the eye
        FindObjectsOfType<MonoBehaviour>()
            .Where(m => m is IMicroGameLoad)
            .Select(m => m as IMicroGameLoad)
            .ForEach(m => m.OnMicroGameLoad());
    }
}