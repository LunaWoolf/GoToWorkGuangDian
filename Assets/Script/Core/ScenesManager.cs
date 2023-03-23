using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ScenesManager : MonoSingleton<ScenesManager>
{
    public static SceneManager Instance { get; private set; }
    public enum SceneType
    {
        None,
        workScene,
        paperShredder,
   
    }

    // Current Scene with a public getter and private setter
    [SerializeField] private SceneType currentScene = SceneType.workScene;
    public SceneType GetCurrentScene() { return currentScene; }
    private void SetCurrentScene(SceneType sceneToSet) { currentScene = sceneToSet; }

    [SerializeField] private SceneType previousScene = SceneType.None;
    private SceneType GetPreviousScene() { return previousScene; }
    private void SetPreviousScene(SceneType sceneToSet) { previousScene = sceneToSet; }


    //variable
    public bool executeSceneLoad = false;

    private UnityEngine.AsyncOperation asyncOperation;
    public bool isSceneBuffering { get; private set; } = false;

    void Awake()
    {
        DontDestroyOnLoad(this);

        //SceneManager.sceneLoaded += OnNewSceneLoaded;
    }

    void Start()
    {
        SetCurrentScene();
        GetCurrentScene();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    public void ChangeSceneByNumber(SceneType sceneToLoad)
    {
        string sceneName = GetSceneNameFromSceneType(sceneToLoad);
        SceneManager.LoadScene(sceneName);
    }

    public SceneType SetCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        string sceneName = scene.name;
        currentScene = GetSceneTypeFromSceneName(sceneName);
        Debug.Log("Scene set to: " + currentScene.ToString());
        return currentScene;
    }

    private void OnNewSceneLoaded(string scene)
    {
        Debug.Log("Loaded new current scene: " + scene + " while previous scene is " + GetCurrentScene().ToString());

        //ApplicationManager.currentState = ApplicationManager.AppState.sceneLoaded;
        //Debug.Log(ApplicationManager.currentState);
      
        // set previous scene and unload it
        SetPreviousScene(GetCurrentScene());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        UnloadPreviousScene();
       
        // set current scene
        SetCurrentScene(GetSceneTypeFromSceneName(scene));
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        //UnloadPreviousScene();
        //ApplicationManager.onSceneLoaded();

    }


    public void UnloadPreviousScene()
    {
        Debug.Log("Previous Scene is:" + GetPreviousScene());
        if (GetPreviousScene() != SceneType.None)
        {
            string sceneToUnLoad = GetSceneNameFromSceneType(GetPreviousScene());
            Scene prevScene = SceneManager.GetSceneByName(sceneToUnLoad);
            if (prevScene.isLoaded)
                SceneManager.UnloadSceneAsync(sceneToUnLoad);
        }
    }


    //Parse Scene name for the scene type and match it with the enum
    public SceneType GetSceneTypeFromSceneName(string sceneName)
    {
        // add leading underscore for enum parsing
        //sceneName = sceneName.Insert(0, "_");

        SceneType currentSceneType;
        if (SceneType.TryParse(sceneName, out currentSceneType))
        {
            return currentSceneType;
        }
        else
        {
            UnityEngine.Debug.Log("unable to parse scene name: " + sceneName + ". is it listed in the SceneType enum?");
            return SceneType.None;
        }
    }

    private string GetSceneNameFromSceneType(SceneType type)
    {
        string sceneName = type.ToString();
        //string sceneName = enumAsString.Remove(0, 1);
        return sceneName;
    }

    // called to start loading next scene's assets in background thread
    public void StartBufferingScene(SceneType sceneToBuffer)
    {
        if (isSceneBuffering) return;
        string sceneName = GetSceneNameFromSceneType(sceneToBuffer);
        StartCoroutine(BufferScene(sceneName));
        UnityEngine.Debug.Log("Started buffering scene: " + sceneName);
    }

    IEnumerator BufferScene(string SceneName)
    {
        isSceneBuffering = true;

        asyncOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncOperation.isDone)
        {
            if (IsBufferedSceneReadyToLoad() && executeSceneLoad)
            {
                //ApplicationManager.loadNextScene();
                asyncOperation.allowSceneActivation = true;
                OnNewSceneLoaded(SceneName);
              
            }
            //Debug.Log(asyncOperation.progress);
            yield return null;
        }
        isSceneBuffering = false;
        executeSceneLoad = false;

    }

    public void LoadBufferedScene()
    {
        executeSceneLoad = true;
    }

    private bool IsBufferedSceneReadyToLoad()
    {
        if (asyncOperation.progress >= .9f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // called to visually execute the loading of a buffered scene
    public void ActivateBufferedScene()
    {
        // unload current streamed level
        // allow scene activation for queued level
    }

    public void UnloadScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }

    public void ResetGame()
    {
#if !UNITY_EDITOR
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
#endif
    }
}
