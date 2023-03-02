using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{

    GameMode currentGameMode;

    public struct personalBannedWord
    {
        public string word;
        public int count;
    }

    public enum Character
    {
        BossHe,
        Li
    }

    public enum GameMode
    {
        Work,
        Moyu,
        Conversation,
        Home
    }

    public GameObject eventSystem;

    [Header("Word Day")]
    public DateTime gameDate = new DateTime(2019, 6, 6, 0, 0, 0);
    public int poemViewedToday = 0 ;
    public int MaxPoemNeedToViewToday = 5;
    [SerializeField] public int denyPoemCount = 0;
    [SerializeField] public int passPoemCount = 0;
    [SerializeField] int WorkActionCountOfDay = 0;
    [SerializeField] int MaxWorkActionCountOfDay = 5;

    [Header("After Work Day")]
    int AfterWorkActionCountOfDay = 0;
    int MaxAfterWorkActionCountOfDay = 1;

    List<personalBannedWord> personalBannedWordList = new List<personalBannedWord>();
    public Dictionary<string, personalBannedWord> personalBannedWordMap = new Dictionary<string, personalBannedWord>();

    public void SetCurrentGameMode(GameMode mode)
    {  currentGameMode = mode;

        switch (currentGameMode)
        {
            case GameMode.Work:
               //StartWork();
               break;
        }
    }

    public GameMode GetCurrentGameMode() { return currentGameMode; }


    [YarnCommand("StartWork")]
    public void TryStartWork()
    {
        StartWork();
    }


    public void StartWork()
    {
        SetCurrentGameMode(GameMode.Work);
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadWorkView();
        PoemGenerator.instance.TearPoem();
        PoemGenerator.instance.GeneratorPoem(5);
    }

    public void StartMoyu()
    {
        SetCurrentGameMode(GameMode.Moyu);
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadMoyuView();
    
    }

    public void GoBackToWork()
    {
        SetCurrentGameMode(GameMode.Work);
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadWorkView();
    }

    void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
        Debug.Log(gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2"));

        personalBannedWord bannedWord = new personalBannedWord();
        bannedWord.word = "Default";
        bannedWord.count = 0;

        personalBannedWordMap.Add("Default", bannedWord);

        ViewManager.instance.UnloadWorkView();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartPaperShredder();
        }

    }


    public bool AdjustAndCheckWorkActionCountOfDay(int x)
    {
        WorkActionCountOfDay += x;
        if (WorkActionCountOfDay >= MaxWorkActionCountOfDay) // if run out of action count at work
        {
            EndOfWorkDay();
            return false;
        }

        return true;
    }

    public void OnPoemPass()
    {
        passPoemCount++;
        if (AdjustAndCheckWorkActionCountOfDay(1))
            TryGoToNextPoem();
       
    }

    public void OnPoemDeny()
    {
        denyPoemCount++;
        if(AdjustAndCheckWorkActionCountOfDay(1))
            TryGoToNextPoem();
    }

    void TryGoToNextPoem()
    {
        PoemGenerator.instance.NextPoem();
      
    }

    void EndOfWorkDay()
    {
        PoemGenerator.instance.UnloadPoemPaper();
        ViewManager.instance.UnloadWorkView();
        LoadEndOfWorkDayDialogue();
        Debug.Log("End of day");
    }

    void LoadEndOfWorkDayDialogue()
    {
        string date = gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2");
      
        LocalDialogueManager.instance.LoadDialogue("d" + date + "_EndWork");

    }

    void LoadMorningWorkDayDialogue()
    {
        string date = gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2");

        LocalDialogueManager.instance.LoadDialogue("d" + date + "_Morning");
        FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true);
    }

    [YarnCommand("GoToNextDay")]
    public void GoToNextWorkDay()
    {

        SceneManager.UnloadSceneAsync("paperShredder");
        
        Debug.Log("Go To Next Day");
        gameDate = gameDate.AddDays(1);
        WorkActionCountOfDay = 0;
        LoadMorningWorkDayDialogue();

        FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true); 
    }

    [YarnCommand("StartPaperShredder")]
    void StartPaperShredder()
    {
        SceneManager.LoadScene("paperShredder", LoadSceneMode.Additive);
        FindObjectOfType<EventSystem>().gameObject.SetActive(false);

    }

    public void CircledWord(string word)
    {
        if (personalBannedWordMap.ContainsKey(word))
        {
            personalBannedWord bannedWord = personalBannedWordMap[word];
            bannedWord.count++;
            personalBannedWordMap[word] = bannedWord;
        }
        else
        {
            personalBannedWord bannedWord = new personalBannedWord();
            bannedWord.word = word;
            bannedWord.count = 0;

            personalBannedWordMap.Add(word, bannedWord);
        }
    }
}
