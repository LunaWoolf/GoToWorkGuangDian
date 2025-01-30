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

    bool hasFadeToBlack = false;
    // Current Scene with a public getter and private setter
    [SerializeField] private SceneType currentScene = SceneType.workScene;
    public SceneType GetCurrentScene() { return currentScene; }
    private void SetCurrentScene(SceneType sceneToSet) { currentScene = sceneToSet; }

    [SerializeField] private SceneType previousScene = SceneType.None;
    private SceneType GetPreviousScene() { return previousScene; }
    private void SetPreviousScene(SceneType sceneToSet) { previousScene = sceneToSet; }

    private string PreviousAdditiveScene = "none";

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

        
        SetPreviousScene(GetCurrentScene());
        if(SceneManager.GetSceneByName(scene).isLoaded)
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        UnloadPreviousScene();
        SetCurrentScene(GetSceneTypeFromSceneName(scene));
        PreviousAdditiveScene = scene;
        ViewManager.instance.FadeBack();


    }


    public void UnloadPreviousScene()
    {
        Debug.Log("Previous Scene is:" + PreviousAdditiveScene);

        if (PreviousAdditiveScene == "paperShredder") return;

        Scene prevScene = SceneManager.GetSceneByName(PreviousAdditiveScene);
        if (prevScene.isLoaded)
            SceneManager.UnloadSceneAsync(prevScene);
        
    }

    

    //Parse Scene name for the scene type and match it with the enum
    public SceneType GetSceneTypeFromSceneName(string sceneName)
    {

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
    public void StartBufferingScene(string sceneToBuffer)
    {
        isSceneBuffering = true;
        asyncOperation = SceneManager.LoadSceneAsync(sceneToBuffer, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        UnityEngine.Debug.Log("Started buffering scene: " + sceneToBuffer);
    }

    public IEnumerator BufferScene()
    {
      
        ViewManager.instance.FadeToBlack();
        yield return new WaitForSeconds(2f);
      
        // Wait until the asynchronous scene fully loads
        while (isSceneBuffering)
        {
            if (IsBufferedSceneReadyToLoad() && executeSceneLoad)
            {
                //ApplicationManager.loadNextScene();
                asyncOperation.allowSceneActivation = true;
                //OnNewSceneLoaded(SceneName);

            }
            //Debug.Log(asyncOperation.progress);
            yield return null;
        }
        //ViewManager.instance.FadeBack();
        isSceneBuffering = false;
        executeSceneLoad = false;

    }


    public void LoadBufferedScene()
    {
        StartCoroutine(BufferScene());
    }

    private bool IsBufferedSceneReadyToLoad()
    {
        if (asyncOperation.progress >= .9f)
        {
            executeSceneLoad = true;
            return true;
        }
        else
        {
            return false;
        }
    }


    public void UnloadScene(string scene)
    {
        StartCoroutine(UnloadScene_IE(scene));

    }

    IEnumerator UnloadScene_IE(string scene)
    {

        ViewManager.instance.FadeToBlack();
        yield return new WaitForSeconds(2f);
        SceneManager.UnloadSceneAsync(scene);
        yield return new WaitForSeconds(.2f);
        ViewManager.instance.FadeBack();
    }

    public void ResetGame()
    {
#if !UNITY_EDITOR
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
#endif
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fade back once the new scene is loaded
        ViewManager.instance.FadeBack();
    }

    // Register this function with the scene load event
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
