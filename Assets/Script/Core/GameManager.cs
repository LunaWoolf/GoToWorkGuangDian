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
        Write,
        //News,
        Conversation,
        Afterwork
    }

    public GameObject eventSystem;

    [Header("Word Day")]
    public DateTime gameDate = new DateTime(2019, 6, 6, 0, 0, 0);
    [SerializeField] int dayCounter = 0;
    public int poemViewedToday = 0 ;
    [SerializeField] int PoemViewedToday = 0;
    [SerializeField] public int denyPoemCount = 0;
    [SerializeField] public int passPoemCount = 0;
    [SerializeField] public int WorkActionCountOfDay = 0;
    [SerializeField] public int MaxWorkActionCountOfDay = 5;
    public UnityEvent onAction;

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

        //temp
        if(FindObjectOfType<WorkViewController>() != null)
            FindObjectOfType<WorkViewController>().InitalActionCount(GameManager.instance.MaxWorkActionCountOfDay - GameManager.instance.WorkActionCountOfDay);

        if (PropertyManager.instance.bHasWritePoem)
        {
           // PoemGenerator.instance.MoveWritePoemToReadPoem();
            //PoemGenerator.instance.GeneratorPoem(5);
        }
        else
        {
            PoemGenerator.instance.GeneratorPoem(5);
        }
           
      
    }

    public void StartWrite()
    {
        SetCurrentGameMode(GameMode.Write);
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadTutorialView("Drag and Drop Words On To  the Paper To Write");
        /*if (!PropertyManager.instance.hasShownWorkTutorial)
        {
            ViewManager.instance.LoadTutorialView("Tutorial_Write");
            //PropertyManager.instance.hasShownWorkTutorial = true;
        }*/
        ViewManager.instance.LoadWriteView();
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
            //ViewManager.instance.LoadTutorialView("News");
            ViewManager.instance.LoadTutorialView("Read News on your phone can help you have a better understanding about the work around you. \n But it will also take up your work time quickly. \n Be careful, you don't want to loose your job right now.");
            PropertyManager.instance.hasShownNewsTutorial = true;
        }
        ViewManager.instance.LoadNewsView();
    }

    public void GoBackToWork()
    {
        AdjustAndCheckWorkActionCountOfDay(0);
        {
            SetCurrentGameMode(GameMode.Work);
            ViewManager.instance.UnloadAllView();
            ViewManager.instance.LoadWorkView();
        }
    }

    void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();

        if (objs.Length > 1)
        {
            foreach (var v in objs)
            {
                if (v.gameObject != this.gameObject)
                    Destroy(v.gameObject);
            }
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
        if(x > 0) onAction.Invoke();
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
        if (PropertyManager.instance.bHasWritePoem)
        {
            LoadEndGameScene();
            FindObjectOfType<PoemPaperController>().OnPoemPass();
            return;
        }

        if (temp_CircledWordList.Count == 0)
        {
            passPoemCount++;
            PoemViewedToday++;
            FindObjectOfType<PoemPaperController>().OnPoemPass();
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
        if (PropertyManager.instance.bHasWritePoem)
        {
            LoadEndGameScene();
            FindObjectOfType<PoemPaperController>().OnPoemDeny();
            return;
        }

        if (temp_CircledWordList.Count > 0)
        {
            denyPoemCount++;
            PoemViewedToday++;
            FindObjectOfType<PoemPaperController>().OnPoemDeny();
            PoemGenerator.instance.OnPoemDeny();
            SaveCircledWord();
            if (AdjustAndCheckWorkActionCountOfDay(1))
                TryGoToNextPoem();
        }
        else
        {
            ViewManager.instance.LoadTutorialView("You must present your reason for denying a piece. \n Clicked on inappropriate or controversial words to circle them, or let this piece pass ");
        }
    }

    void TryGoToNextPoem()
    {
        PoemGenerator.instance.NextPoem();
    }

    void EndOfWorkDay()
    {
        PoemGenerator.instance.UnloadPoemPaper();
        ViewManager.instance.UnloadWorkView();
        if (!CheckedReachForEnding())
        {
            LoadEndOfWorkDayDialogue();
        }
        Debug.Log("End of day");
    }

    void LoadEndOfWorkDayDialogue()
    {
        string date = gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2");

        if (LocalDialogueManager.instance.IsDialogueExsist("d" + date + "_EndWork"))
        {
            LocalDialogueManager.instance.LoadDialogue("d" + date + "_EndWork");
        }
        else
        {
            LocalDialogueManager.instance.LoadDialogue("default_EndWork");
        }
    }

    void LoadMorningWorkDayDialogue()
    {
        string date = gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2");

        if (LocalDialogueManager.instance.IsDialogueExsist("d" + date + "_Morning"))
        {
            LocalDialogueManager.instance.LoadDialogue("d" + date + "_Morning");
        }
        else
        {
            LocalDialogueManager.instance.LoadDialogue("default_morning");
        }

        FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true);
    }

    public void GoToAfterwork()
    {
        SetCurrentGameMode(GameMode.Afterwork);
        SceneManager.UnloadSceneAsync("paperShredder");
        Debug.Log("Go To After work Day");
        FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true);

        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadAfterWorkView();
    }

    public void GoToNextWorkDay()
    {
        StartCoroutine(GoToNextWorkDayWithFadeToBlack());
    }
    IEnumerator GoToNextWorkDayWithFadeToBlack()
    {
        //SceneManager.UnloadSceneAsync("paperShredder");
        ViewManager.instance.FadeToBlack();
        yield return new WaitForSeconds(1f);
        Debug.Log("Go To Next Day");
        gameDate = gameDate.AddDays(1);
        dayCounter++;
        WorkActionCountOfDay = 0;
        if (!CheckedReachForEnding())
        {
            LoadMorningWorkDayDialogue();
        }
        PoemViewedToday = 0;
        FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true); 
    }

    bool CheckedReachForEnding()
    {
        if (PropertyManager.instance.bHasWritePoem) return false;

        if (PoemViewedToday == 0) // if yesterday all spend on phone
        {
            LocalDialogueManager.instance.LoadDialogue("Ending_LoseJob_Phone");
            return true;
        }
        else if (dayCounter > 9) // replace by ai
        {
            LocalDialogueManager.instance.LoadDialogue("Ending_LoseJob_AI");
            return true;
        }

        return false;
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

    public void LoadEndGameScene()
    {
        StartCoroutine(IE_LoadEndGameScene());
       
    }

    IEnumerator IE_LoadEndGameScene()
    {
        ViewManager.instance.FadeToBlack_end();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Feedback", LoadSceneMode.Single);
    }

    //temp fix
    public void AddWordToWordList(string s)
    {

        personalBannedWord bannedWord = new personalBannedWord();
        bannedWord.word = s;
        bannedWord.count = 0;
        if (!personalBannedWordMap.ContainsKey(s))
        {
            personalBannedWordMap.Add(s, bannedWord);
        }
    }

}
