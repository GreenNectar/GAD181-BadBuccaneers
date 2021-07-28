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
    [SerializeField, Scene]
    private string homeScreen;

    [SerializeField, Scene]
    private List<string> levels = new List<string>();
    private Stack<string> levelsToPlay = new Stack<string>();

    private void Start()
    {
        //? This is for testing, remove this later
        //GenerateRandomLevels();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager.onResultsFinish.AddListener(LoadNextLevel);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventManager.onResultsFinish.RemoveListener(LoadNextLevel);
    }

    public void LoadResultsScreen()
    {
        SceneManager.LoadScene(resultScreen);
    }

    public void LoadNextLevel()
    {
        if (levelsToPlay.Count > 0)
        {
            string level = levelsToPlay.Pop();
            SceneManager.LoadScene(level);
        }
        else
        {
            SceneManager.LoadScene(homeScreen);
        }
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

    //public void SetLevels(List<string> levels)
    //{
    //    levelsToPlay = levels;
    //}

    public void GenerateRandomLevels()
    {
        // Clear the current levels stacked
        levelsToPlay.Clear();

        // How many levels we want to add
        int levelsToAdd = 3;

        // The stack we will be getting the levels from
        //string[] temp = new string[levels.Count];
        //levels.CopyTo(temp);
        List<string> levelStack = levels.ToList();//temp.ToList();

        // Go through each level and randomly add a level then remove it from the stack
        for (int i = 0; i < Mathf.Clamp(levelsToAdd, 0, levels.Count); i++)
        {
            int randomLevelIndex = Random.Range(0, levelStack.Count);
            levelsToPlay.Push(levelStack[randomLevelIndex]);
            levelStack.RemoveAt(randomLevelIndex);
        }
    }
}