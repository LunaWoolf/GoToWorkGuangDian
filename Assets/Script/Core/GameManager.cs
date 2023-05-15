using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using TMPro;

public class GameManager : MonoSingleton<GameManager>
{

    public bool isDebug = false;
    public bool isLoadOpenScene = false;
    [SerializeField] GameMode currentGameMode = GameMode.Conversation;

   
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
        Mom,
        Dad,
        Sister,
    }

    [Serializable]
    public enum GameMode
    {
        Conversation,
        PaperShredder,
        Work,
        //Moyu,
        Write,
        Bus,
        //News,
        Dinner,
        SaySomething,
        Afterwork
    }

    [Serializable]
    public enum WorkMode
    {
       Timer,
       ActionCount,
    }



    public GameObject eventSystem;

    [Header("Word Day")]
    public DateTime gameDate = new DateTime(2019, 6, 6, 0, 0, 0);
    int dayCounter = 0;
    public int poemViewedToday = 0 ;
    [SerializeField] int PoemViewedToday = 0;
    [SerializeField] public int denyPoemCount_total = 0;
    [SerializeField] public int passPoemCount_total = 0;

    [SerializeField] public int denyPoemCount_day = 0;
    [SerializeField] public int passPoemCount_day = 0;
    [SerializeField] public int WorkActionCountOfDay = 0;
    [SerializeField] public int MaxWorkActionCountOfDay = 5;
    [HideInInspector]public UnityEvent onAction;
    [HideInInspector] public UnityEvent onStartWork;
    [HideInInspector] public float WorkDayTimer;
    public float WorkDayTimeLimit;
    public bool  isPauseWorkDayTimer = false;
    public float maxDayLimit;
    [SerializeField] public WorkMode workMode = WorkMode.ActionCount;


    [Header("Money")]
    [SerializeField] public int passPoem_money = 5;
    [SerializeField] public int passPoem_wrongword_money = 3;
    [SerializeField] public int denyPoem_money = 2;
    [SerializeField] public int denyPoem_wrongword_money = 4;

    [Header("After Work Day")]
    int AfterWorkActionCountOfDay = 0;
    int MaxAfterWorkActionCountOfDay = 1;
    [SerializeField] int cigarettePrice = 5;

    [Header("Work Related")]
   
    public List<string> personalBannedWord_Poem = new List<string>();
    public Dictionary<string, personalBannedWord> personalBannedWordMap = new Dictionary<string, personalBannedWord>();
    public List<string> personalBannedWord_Day = new List<string>();


    public void SetCurrentGameMode(GameMode mode)
    {  currentGameMode = mode;

        switch (currentGameMode)
        {
            case GameMode.Work:
               //StartWork();
               break;
            case GameMode.Write:
                ScenesManager.instance.StartBufferingScene("SaySomethingScene");
                break;
            case GameMode.Bus:
                ScenesManager.instance.StartBufferingScene("BusScene");
                break;
            case GameMode.Dinner:
                ScenesManager.instance.StartBufferingScene("DinnerScene");
                break;
        }
    }

    public GameMode GetCurrentGameMode() { return currentGameMode; }

    public string GetGameDate() { return gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2"); }

    public int GetDay() { return dayCounter; }

    public void TryStartWork()
    {
        StartWork();
    }



    public void StartWork()
    {
        SetCurrentGameMode(GameMode.Work);
        onStartWork.Invoke();
        WorkDayTimer = 0;
        passPoemCount_day = 0;
        denyPoemCount_day = 0;
        ViewManager.instance.UnloadAllView();
        if (!PropertyManager.instance.hasShownWorkTutorial)
        {
            //ViewManager.instance.LoadTutorialView("Tutorial_Work");
            PropertyManager.instance.hasShownWorkTutorial = true;
        }
        ViewManager.instance.LoadWorkView();
        PoemGenerator.instance.TearPoem();
        PoemGenerator.instance.TryGoToNextPoem();
        //temp
        if (FindObjectOfType<WorkViewController>() != null)
            FindObjectOfType<WorkViewController>().InitalActionCount(GameManager.instance.MaxWorkActionCountOfDay - GameManager.instance.WorkActionCountOfDay);


        //temp
        if (PropertyManager.instance.bHasWritePoem)
        {
            // PoemGenerator.instance.MoveWritePoemToReadPoem();
            //PoemGenerator.instance.GeneratorPoem(5);
           PropertyManager.instance.bHasWritePoem = false;
           PoemGenerator.instance.TearPoemAfterWrite();
        }
        else
        {
           PoemGenerator.instance.GeneratorPoem(5);
        }

        personalBannedWord_Day.Clear();
    }

    public void StartWrite()
    {
        SetCurrentGameMode(GameMode.Write);
        ViewManager.instance.UnloadAllView();
  
        /*if (!PropertyManager.instance.hasShownWorkTutorial)
        {
            ViewManager.instance.LoadTutorialView("Tutorial_Write");
            //PropertyManager.instance.hasShownWorkTutorial = true;
        }*/
        ViewManager.instance.LoadWriteView();
    }

    public void StartMoyu()
    {
        //SetCurrentGameMode(GameMode.Moyu);
        //ViewManager.instance.UnloadAllView();
        //ViewManager.instance.LoadMoyuView();
    
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
        /*var objs = FindObjectsOfType<GameManager>();

        if (objs.Length > 1)
        {
            foreach (var v in objs)
            {
                if (v.gameObject != this.gameObject)
                    Destroy(v.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);*/
        
    }

    void Start()
    {

        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        Debug.Log("starttt");

        if(!isDebug || isLoadOpenScene)
            SceneManager.LoadScene("OpenScene", LoadSceneMode.Additive);

        Debug.Log(gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2"));

        personalBannedWord bannedWord = new personalBannedWord();
        bannedWord.word = "Default";
        bannedWord.count = 0;

        personalBannedWord_Day.Add("Default");
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

        //increase Timer
        if (!isPauseWorkDayTimer && currentGameMode == GameMode.Work)
        {
            if (workMode == WorkMode.Timer)
            {
                WorkDayTimer += Time.deltaTime;
                ViewManager.instance.SetTimerText(WorkDayTimeLimit - WorkDayTimer);
                if (WorkDayTimer > WorkDayTimeLimit)
                {
                    EndOfWorkDay();
                }
            }
            else
            {
                //turn off timer text
            }
            
        }

     
    }


    public bool AdjustAndCheckWorkActionCountOfDay(int x)
    {
        //Stop using work action for now

        if (workMode == WorkMode.ActionCount)
        {
            WorkActionCountOfDay += x;
            if (x > 0) onAction.Invoke();
            if (WorkActionCountOfDay >= MaxWorkActionCountOfDay) // if run out of action count at work
            {
                PoemGenerator.instance.UnloadPoemPaper();
                StartCoroutine(WaitToEndWork());
                return false;
            }

            return true;
        }

        //Not using action count mode 
        return true;
    }

    IEnumerator WaitToEndWork()
    {
        yield return new WaitForSeconds(1f);
        EndOfWorkDay();
    }

    public void OnPoemTryPass()
    {
        /*if (PropertyManager.instance.bHasWritePoem)
        {
            LoadEndGameScene();
            FindObjectOfType<PoemPaperController>().OnPoemPass();
            return;
        }*/

        if (personalBannedWord_Poem.Count == 0)
        {
            passPoemCount_total++;
            passPoemCount_day++;
            PoemViewedToday++;
            FindObjectOfType<PoemPaperController>().OnPoemPass();
            PoemGenerator.instance.OnPoemPass();

            PropertyManager.instance.UpdateMoney(passPoem_money); // Gain money for pass poem
            PropertyManager.instance.UpdateMoney(-PropertyManager.instance.rebelliousCount * passPoem_wrongword_money); // Lose money for each word that does not suppose to be passed
            PropertyManager.instance.rebelliousCount = 0;

           
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
        // Load Ending // Temp logic 
        /*if (PropertyManager.instance.bHasWritePoem)
        {
            LoadEndGameScene();
            FindObjectOfType<PoemPaperController>().OnPoemDeny();
            return;
        }*/

        if (personalBannedWord_Poem.Count > 0)
        {
            denyPoemCount_total++;
            denyPoemCount_day++;
            PoemViewedToday++;
            FindObjectOfType<PoemPaperController>().OnPoemDeny(); //Turn on stamp
            PoemGenerator.instance.OnPoemDeny();
            SaveCircledWordForPoem();
            PropertyManager.instance.UpdateMoney(denyPoem_money);
            PropertyManager.instance.UpdateMoney(-PropertyManager.instance.rebelliousCount * denyPoem_wrongword_money); // didn't circle the word that should circle
            PropertyManager.instance.rebelliousCount = 0;
      
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
        SaveTodayWorkBannedWorkToBannedWordDictionary();
        SetCurrentGameMode(GameMode.PaperShredder);
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

        /*if (PoemViewedToday == 0) // if yesterday all spend on phone
        {
            //LocalDialogueManager.instance.LoadDialogue("Ending_LoseJob_Phone");
            return true;
        }*/

        if (dayCounter > maxDayLimit) // replace by ai
        {
            LocalDialogueManager.instance.LoadDialogue("Ending_LoseJob_AI");
            return true;
        }

        return false;
    }


    public void StartPaperShredder()
    {
        //ScenesManager.instance.StartBufferingScene("paperShredder");
        //SceneManager.LoadScene("paperShredder", LoadSceneMode.Additive);
        //FindObjectOfType<EventSystem>().gameObject.SetActive(false);

        FindObjectOfType<PaperShredderManager>().StartPaperShredder();

    }

    public void CircledWordInCurrentPoem(string word)
    {
        personalBannedWord_Poem.Add(word);
    }

    public void CancleCircledWordInCurrentPoem(string word)
    {
        if(personalBannedWord_Poem.Contains(word))
           personalBannedWord_Poem.Remove(word);

    }

  
    public void SaveCircledWordForPoem()
    {
        foreach (string s in personalBannedWord_Poem)
        {
            personalBannedWord_Day.Add(s);
        }

        personalBannedWord_Poem.Clear();
    }

    public void SaveTodayWorkBannedWorkToBannedWordDictionary()
    {
        foreach (string s in personalBannedWord_Day)
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
        SceneManager.LoadScene("End_Poem", LoadSceneMode.Single);
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


    public void GoToBus()
    {
        SetCurrentGameMode(GameMode.Bus);
        Debug.Log("Go To Bus");
        if(FindObjectOfType<EventSystem>()) FindObjectOfType<EventSystem>().gameObject.SetActive(false);
        eventSystem.SetActive(false);
        ViewManager.instance.UnloadAllView();
    }

    public void GoToDinner()
    {
        SetCurrentGameMode(GameMode.Dinner);
        Debug.Log("Go To Dinner");
        if (FindObjectOfType<EventSystem>()) FindObjectOfType<EventSystem>().gameObject.SetActive(false);
        eventSystem.SetActive(false);
        ViewManager.instance.UnloadAllView();
    }

    public void GoToAfterwork()
    {
        SetCurrentGameMode(GameMode.Afterwork);
        Debug.Log("Go To After work Day");
        if (FindObjectOfType<EventSystem>()) FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true);
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadAfterWorkView();
    }

    public bool BuyCigarette()
    {
        if (PropertyManager.instance.GetMoney() < cigarettePrice) return false;

        PropertyManager.instance.UpdateMoney(-cigarettePrice);
        PropertyManager.instance.cigaretteCount += 1;
        return true;

    }

    public void  WaitForFinishRepeatMission()
    {
        LocalDialogueManager.instance.LoadDialogue("FirstDay_Morning_afterRepeatMission");
       
    }

}

//"Circle the words you want to revise, then click the button to request a revision. Some words are crucial and the author will refuse your revision request."
//"Circle the words you consider should be censored, then reject this piece. You will be penalized if you fail to select words that are supposed to be censored."
//"Make sure the piece has zero controversial words before you give it a pass. You can always ask for it to be revised. You will be penalized if you pass a piece that contains words that are supposed to be censored."