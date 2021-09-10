using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Scenes")]
    [SerializeField, Scene]
    private string resultScreen;
    [SerializeField, Scene]
    private string homeScreen;
    [SerializeField, Scene]
    private string microGameOverlay;
    [SerializeField, Scene]
    private string playerUIOverlay;

    [Header("Levels")]
    [SerializeField]
    private List<MicroGame> levels = new List<MicroGame>();
    private Stack<MicroGame> levelsToPlay = new Stack<MicroGame>();
    public MicroGame currentMicroGame { get; private set; } = null;

    private int numberOfLevels;

    public bool IsInPracticeMode
    {
        get
        {
            return SceneManager.GetSceneByName(microGameOverlay).IsValid();
        }
    }

    //private void Start()
    //{
    //    //? This is for testing, remove this later
    //    GenerateRandomLevels();
    //}

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

    public void SetLevels(int level)
    {
        numberOfLevels = level;
    }

    /// <summary>
    /// Loads the results screen...
    /// </summary>
    public void LoadResultsScreen()
    {
        SceneManager.LoadScene(resultScreen);
    }

    /// <summary>
    /// Loads the next level
    /// </summary>
    public void LoadNextLevel()
    {
        if (levelsToPlay.Count > 0)
        {
            currentMicroGame = levelsToPlay.Pop();
            string level = currentMicroGame.microGameScene;
            SceneManager.LoadScene(level, LoadSceneMode.Single);
            SceneManager.LoadScene(microGameOverlay, LoadSceneMode.Additive);
            SceneManager.LoadScene(playerUIOverlay, LoadSceneMode.Additive);
        }
        else
        {
            ScoreManager.Instance.ResetEverything();
            SceneManager.LoadScene(homeScreen);
        }
    }

    /// <summary>
    /// Loads the level without the overlay
    /// </summary>
    public void LoadOutOfPractice()
    {
        StartCoroutine(LoadOutOfPracticeSequence());
    }

    /// <summary>
    /// Waits a bit before loading the level without the overlay
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadOutOfPracticeSequence()
    {
        yield return new WaitForSeconds(1f);

        string level = currentMicroGame.microGameScene;
        SceneManager.LoadScene(level, LoadSceneMode.Single);
        SceneManager.LoadScene(playerUIOverlay, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Calls the interface, this was for testing and I don't think this is needed
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
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

    /// <summary>
    /// Generates random levels using the level stack, no duplicates
    /// </summary>
    public void GenerateRandomLevels()
    {
        // Reset all score stuff
        ScoreManager.Instance.ResetEverything();

        // Clear the current levels stacked
        levelsToPlay.Clear();

        // The stack we will be getting the levels from
        //string[] temp = new string[levels.Count];
        //levels.CopyTo(temp);
        List<MicroGame> levelStack = levels.ToList();//temp.ToList();

        // Go through each level and randomly add a level then remove it from the stack
        for (int i = 0; i < Mathf.Clamp(numberOfLevels, 0, levels.Count); i++)
        {
            int randomLevelIndex = Random.Range(0, levelStack.Count);
            levelsToPlay.Push(levelStack[randomLevelIndex]);
            levelStack.RemoveAt(randomLevelIndex);
        }
    }

    /// <summary>
    /// This is the static version for invoking
    /// </summary>
    public static void EndGameStatic(float time = 0f)
    {
        Instance.EndGame(time);
    }

    
    public void EndGame(float time = 0f)
    {
        if (time > 0f)
            StartCoroutine(EndGameRoutine(time));
        else
            EndGame();
    }

    private IEnumerator EndGameRoutine(float time = 0f)
    {
        yield return new WaitForSeconds(time);
        EndGame();
    }

    /// <summary>
    /// End the game. If in practice mode it restarts the level, otherwise it loads the results screen
    /// </summary>
    private void EndGame()
    {
        if (IsInPracticeMode)
        {
            StartCoroutine(ReloadSceneSequence());
        }
        else
        {
            ScoreManager.Instance.EndFinalPlayers();
            ScoreManager.Instance.FinaliseScores();
            LoadResultsScreen();
            EventManager.onGameEnd.Invoke();
        }
    }

    private IEnumerator ReloadSceneSequence()
    {
        yield return SceneManager.UnloadSceneAsync(currentMicroGame.microGameScene);
        yield return SceneManager.LoadSceneAsync(currentMicroGame.microGameScene, LoadSceneMode.Additive);
        FindObjectOfType<MicroGameOverlayController>().SetCamera();
    }
}
