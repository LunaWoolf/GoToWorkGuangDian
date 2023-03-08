using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class GameManager : MonoSingleton<GameManager>
{

    public bool isDebug = false;
    GameMode currentGameMode;

    public struct personalBannedWord
    {
        public string word;
        public int count;
    }

    public enum Character
    {
        BossHe,
        Li,
        CATgpt,
        You,
    }

    public enum GameMode
    {
        Work,
        Moyu,
        //News,
        Conversation,
        Afterwork
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

    [Header("Work Related")]
   
    public List<string> temp_CircledWordList = new List<string>();
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


   
    public void TryStartWork()
    {
        StartWork();
    }


    public void StartWork()
    {
        SetCurrentGameMode(GameMode.Work);
        ViewManager.instance.UnloadAllView();
        if (!PropertyManager.instance.hasShownWorkTutorial)
        {
            ViewManager.instance.LoadTutorialView("Tutorial_Work");
            PropertyManager.instance.hasShownWorkTutorial = true;
        }
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

    public void StartNews()
    {
        //SetCurrentGameMode(GameMode.News);
        ViewManager.instance.UnloadAllView();
        if (!PropertyManager.instance.hasShownNewsTutorial)
        {
            ViewManager.instance.LoadTutorialView("News");
            PropertyManager.instance.hasShownNewsTutorial = true;
        }
       
        ViewManager.instance.LoadNewsView();
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

        if(ViewManager.instance != null)
            ViewManager.instance.UnloadWorkView();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDebug && Input.GetKeyDown(KeyCode.H))
        {
            StartPaperShredder();
        }

        if (isDebug && Input.GetKeyDown(KeyCode.N))
        {
            NewsManager.instance.RefreshCureentValidNews();
            NewsManager.instance.GeneratreNews();
        }

    }


    public bool AdjustAndCheckWorkActionCountOfDay(int x)
    {
        WorkActionCountOfDay += x;
        if (WorkActionCountOfDay >= MaxWorkActionCountOfDay) // if run out of action count at work
        {
            PoemGenerator.instance.UnloadPoemPaper();
            StartCoroutine(WaitToEndWork());
            return false;
        }

        return true;
    }

    IEnumerator WaitToEndWork()
    {
        yield return new WaitForSeconds(1f);
        EndOfWorkDay();
    }

    public void OnPoemTryPass()
    {
        if (temp_CircledWordList.Count == 0)
        {
            passPoemCount++;
            PoemPaperController.instance.OnPoemPass();
            PoemGenerator.instance.OnPoemPass();
            if (AdjustAndCheckWorkActionCountOfDay(1))
                TryGoToNextPoem();
            
        }
        else
        {
            ViewManager.instance.LoadTutorialView("Lyric with controversial words you selected cannot pass. \n Reconsider your choice or deny this piece");
        }
    }

    public void OnPoemTryDeny()
    {
        denyPoemCount++;
        PoemPaperController.instance.OnPoemDeny();
        PoemGenerator.instance.OnPoemDeny();
        SaveCircledWord();
        if (AdjustAndCheckWorkActionCountOfDay(1))
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

    public void GoToAfterwork()
    {
        SceneManager.UnloadSceneAsync("paperShredder");
        Debug.Log("Go To After work Day");
        FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true);

        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadAfterWorkView();
    }

    public void GoToNextWorkDay()
    {
        //SceneManager.UnloadSceneAsync("paperShredder");
        
        Debug.Log("Go To Next Day");
        gameDate = gameDate.AddDays(1);
        WorkActionCountOfDay = 0;
        LoadMorningWorkDayDialogue();

        FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true); 
    }

  
    public void StartPaperShredder()
    {
        SceneManager.LoadScene("paperShredder", LoadSceneMode.Additive);
        FindObjectOfType<EventSystem>().gameObject.SetActive(false);

    }

    public void CircledWord(string word)
    {
        temp_CircledWordList.Add(word);
    }

    public void CancleCircledWord(string word)
    {
        if(temp_CircledWordList.Contains(word))
            temp_CircledWordList.Remove(word);
    }

    public void SaveCircledWord()
    {
        foreach (string s in temp_CircledWordList)
        {
            if (personalBannedWordMap.ContainsKey(s))
            {
                personalBannedWord bannedWord = personalBannedWordMap[s];
                bannedWord.count++;
                personalBannedWordMap[s] = bannedWord;
            }
            else
            {
                personalBannedWord bannedWord = new personalBannedWord();
                bannedWord.word = s;
                bannedWord.count = 0;

                personalBannedWordMap.Add(s, bannedWord);
            }
        }

        temp_CircledWordList.Clear();
    }

    
    public string GetRandomBannedWord()
    {

        string s = "Death";
        if (this.personalBannedWordMap.Count > 1)// cause the first item in map is "Default
        {
            List<string> keyList = new List<string>(this.personalBannedWordMap.Keys);
            int i = UnityEngine.Random.Range(1, personalBannedWordMap.Keys.Count);
            s = keyList[i];

        }

        return s;
    }
}
