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
        //Write,
        Bus,
        //News,
        Dinner,
        SaySomething,
        //Afterwork,
        Lake,
        Dream,
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

    [Header("Word Day")]
    [SerializeField] int PoemViewedToday = 0;
    [SerializeField] public int passPoemCount_total = 0;
    [HideInInspector] public int passPoemCount_day = 0;
    [SerializeField] public int MaxWorkActionCountOfDay = 5;
    [HideInInspector] public int WorkActionCountOfDay = 0;
    [HideInInspector] public int denyPoemCount_total = 0;
    [HideInInspector] public int denyPoemCount_day = 0;
    [HideInInspector] public UnityEvent onAction;
    [HideInInspector] public UnityEvent onStartWork;
    [HideInInspector] public float WorkDayTimer;
    [HideInInspector] public float WorkDayTimeLimit;
    [HideInInspector] public bool isPauseWorkDayTimer = false;
    [SerializeField] public WorkMode workMode = WorkMode.ActionCount;

    [Header("Mission Reference")]
    [SerializeField] GameObject[] Mission_Day;
    [TextArea(2,10)]public string[] MissionLine;


    [Header("Money")]
    [SerializeField] public int passPoem_money = 5;
    [SerializeField] public int passPoem_wrongword_money = 3;
    [HideInInspector] public int denyPoem_money = 2;
    [HideInInspector] public int denyPoem_wrongword_money = 4;

    [Header("After Work Day")]
    int AfterWorkActionCountOfDay = 0;
    int MaxAfterWorkActionCountOfDay = 1;
    [SerializeField] int cigarettePrice = 5;

    [Header("Work Related")]

    public List<string> personalBannedWord_Poem = new List<string>();
    public Dictionary<string, personalBannedWord> personalBannedWordMap = new Dictionary<string, personalBannedWord>();
    public List<string> personalBannedWord_Day = new List<string>();

    [HideInInspector] public UnityEvent OnPoemPass;
    [HideInInspector] public UnityEvent OnPoemPassFailed;


    [Header("Dialogue")]
    public bool LoadTestDialogue = false;

    public void SetCurrentGameMode(GameMode mode)
    {
        currentGameMode = mode;

        switch (currentGameMode)
        {
            case GameMode.Work:
                //StartWork();
                break;
            case GameMode.SaySomething:
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

    public void SetDay(int day) { dayCounter = day; }

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
        ViewManager.instance.TogglePropertyCanvas(true);

        if (!PropertyManager.instance.hasShownWorkTutorial)
        {
            //ViewManager.instance.LoadTutorialView("Tutorial_Work");
            PropertyManager.instance.hasShownWorkTutorial = true;
        }
        ViewManager.instance.LoadWorkView();
        ViewManager.instance.UnloadTipView();
        ViewManager.instance.LoadTipView(TipViewController.TipType.WorkTip);
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

    public void StartSaySomething()
    {
        SetCurrentGameMode(GameMode.SaySomething);
        ViewManager.instance.UnloadAllView();

        /*if (!PropertyManager.instance.hasShownWorkTutorial)
        {
            ViewManager.instance.LoadTutorialView("Tutorial_Write");
            //PropertyManager.instance.hasShownWorkTutorial = true;
        }*/
        ViewManager.instance.LoadWriteView();
        ViewManager.instance.TogglePropertyCanvas(true);
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
        LeanTween.init(1000);
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        Debug.Log("starttt");

        //if (!isDebug || isLoadOpenScene)
           //SceneManager.LoadScene("OpenScene", LoadSceneMode.Additive);

        Debug.Log(gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2"));

        personalBannedWord bannedWord = new personalBannedWord();
        bannedWord.word = "Default";
        bannedWord.count = 0;

        personalBannedWord_Day.Add("Default");
        personalBannedWordMap.Add("Default", bannedWord);

        if (ViewManager.instance != null)
            ViewManager.instance.UnloadWorkView();


    }

    // Update is called once per frame
    void Update()
    {

      
        /*
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

        }*/


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
        if (/*personalBannedWord_Poem.Count == 0*/ true)
        {
            if (isDebug)
            {
                Debug.Log("Debug Pass for flow test");
                PoemPass();
                return;
            }
            if (PropertyManager.instance.currentPoemBannedWord > 0)
            {
                PoemPassFailed();
            }
            else
            {
                PoemPass();
            }

        }
        /*else
        {
            ViewManager.instance.LoadTutorialView("Lyric with potential controversial words you selected cannot pass. \n Reconsider your choice or deny this piece");
        }*/
    }

    public void PoemPassFailed()
    {
        //Boss Give Comment
        //Disable pass button
        OnPoemPassFailed.Invoke();
    }


    public void PoemPass()
    {
        passPoemCount_total++;
        passPoemCount_day++;
        PoemViewedToday++;

        OnPoemPass.Invoke();

        PropertyManager.instance.UpdateMoney(passPoem_money); // Gain money for pass poem
        PropertyManager.instance.UpdateMoney(-PropertyManager.instance.currentPoemBannedWord * passPoem_wrongword_money); // Lose money for each word that does not suppose to be passed
        //PropertyManager.instance.rebelliousCount = 0;
        PropertyManager.instance.currentPoemBannedWord = 0;

        if (AdjustAndCheckWorkActionCountOfDay(1))
            TryGoToNextPoem();

    }

    public void OnPoemTryDeny()
    {
        if (personalBannedWord_Poem.Count > 0)
        {
            denyPoemCount_total++;
            denyPoemCount_day++;
            PoemViewedToday++;
            FindObjectOfType<PoemPaperController>().OnPoemDeny(); //Turn on stamp
            PoemGenerator.instance.OnPoemDeny();
            SaveCircledWordForPoem();
            PropertyManager.instance.UpdateMoney(denyPoem_money);
            PropertyManager.instance.UpdateMoney(-PropertyManager.instance.currentPoemBannedWord * denyPoem_wrongword_money); // didn't circle the word that should circle
            //PropertyManager.instance.rebelliousCount = 0;
            PropertyManager.instance.currentPoemBannedWord = 0;

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
        //string date = gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2");
        SetCurrentGameMode(GameMode.Conversation);
        if (LocalDialogueManager.instance.IsDialogueExsist("d0" + (GetDay() + 1) + "_EndWork"))
        {
            LocalDialogueManager.instance.LoadDialogue("d0" + (GetDay() + 1) + "_EndWork");
        }
        else
        {
            LocalDialogueManager.instance.LoadDialogue("default_EndWork");
        }
    }


    public void LoadMorningWorkDayDialogue()
    {
        //string date = gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2");
        SetCurrentGameMode(GameMode.Conversation);
        Debug.Log("d" + "0" + (GetDay() + 1) + "_Morning");
        if (GetDay() == 0 && LoadTestDialogue)
        {
            LocalDialogueManager.instance.LoadDialogue("d" + "0" + (GetDay() + 1) + "_Morning_Test");
        }
        else if (LocalDialogueManager.instance.IsDialogueExsist("d" + "0" + (GetDay() + 1) + "_Morning"))
        {
            LocalDialogueManager.instance.LoadDialogue("d" + "0" + (GetDay() + 1) + "_Morning");
        }
        else
        {
            LocalDialogueManager.instance.LoadDialogue("default_morning");
        }

        FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true);
        ViewManager.instance.TogglePropertyCanvas(false);
    }

    public bool CanEnterLakeMode() { return LocalDialogueManager.instance.IsDialogueExsist("d" + "0" + (GetDay() + 1) + "_Lake"); }
    public bool CanEnterDreamMode() { return LocalDialogueManager.instance.IsDialogueExsist("d" + "0" + (GetDay() + 1) + "_Dream"); }



    public void GoToNextWorkDay()
    {
        StartCoroutine(GoToNextWorkDayWithFadeToBlack());
        ViewManager.instance.TogglePropertyCanvas(false);
    }

    IEnumerator GoToNextWorkDayWithFadeToBlack()
    {
        //ViewManager.instance.FadeToBlack();
        yield return new WaitForSeconds(.2f);
        Debug.Log("Go To Next Day");
        gameDate = gameDate.AddDays(1);
        SetDay(GetDay() + 1);
        WorkActionCountOfDay = 0;
        PoemViewedToday = 0;
        FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true);
        ViewManager.instance.ToggleDoorButton(true, true, false);
    }


    bool CheckedReachForEnding()
    {
        if (PropertyManager.instance.bHasWritePoem) return false;

        /*if (PoemViewedToday == 0) // if yesterday all spend on phone
        {
            //LocalDialogueManager.instance.LoadDialogue("Ending_LoseJob_Phone");
            return true;
        }*/

        /*if (dayCounter > maxDayLimit) // replace by ai
        {
            LocalDialogueManager.instance.LoadDialogue("Ending_LoseJob_AI");
            return true;
        }*/

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
        if (personalBannedWord_Poem.Contains(word))
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
        if (FindObjectOfType<EventSystem>()) FindObjectOfType<EventSystem>().gameObject.SetActive(false);
        eventSystem.SetActive(false);
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.TogglePropertyCanvas(false);
    }

    public void GoToDinner()
    {
        SetCurrentGameMode(GameMode.Dinner);
        Debug.Log("Go To Dinner");
        if (FindObjectOfType<EventSystem>()) FindObjectOfType<EventSystem>().gameObject.SetActive(false);
        eventSystem.SetActive(false);
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.UnloadTipView();
        ViewManager.instance.LoadTipView(TipViewController.TipType.InteractTip);
        ViewManager.instance.TogglePropertyCanvas(false);
    }

    public void EndBus()
    {
        if (FindObjectOfType<EventSystem>()) FindObjectOfType<EventSystem>().gameObject.SetActive(false);
        eventSystem.SetActive(true);
        ScenesManager.instance.UnloadScene("BusScene");
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.ToggleDoorButton(true, false, true);
    }

    public void GoToSaySomething()
    {
        //SetCurrentGameMode(GameMode.SaySomething);
        Debug.Log("Go To After work Day");
        if (FindObjectOfType<EventSystem>()) FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        eventSystem.SetActive(true);
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadAfterWorkView();
        ViewManager.instance.UnloadTipView();
        ViewManager.instance.LoadTipView(TipViewController.TipType.InteractTip);
        ViewManager.instance.TogglePropertyCanvas(true);
    }

    public void GoToLake()
    {
        SetCurrentGameMode(GameMode.Lake);
        ViewManager.instance.FadeToBlack();
        ScenesManager.instance.UnloadScene("DinnerScene");
        Debug.Log("Go To Lake Mode");
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadLakeView();
        StartCoroutine(LoadLakeDialogueWithDelay());
        ViewManager.instance.UnloadTipView();
        ViewManager.instance.LoadTipView(TipViewController.TipType.DialogueTip);
        ViewManager.instance.TogglePropertyCanvas(false);
    }

    IEnumerator LoadLakeDialogueWithDelay()
    {
        yield return new WaitForSeconds(2f);
        LocalDialogueManager.instance.LoadDialogue("d" + "0" + (GetDay() + 1) + "_Lake");
    }




    public void GoToDream()
    {
        SetCurrentGameMode(GameMode.Dream);
        Debug.Log("Go To Dream");
        //if (FindObjectOfType<EventSystem>()) FindObjectOfType<EventSystem>().gameObject.SetActive(true);
        //eventSystem.SetActive(true);
        ViewManager.instance.UnloadAllView();
        LocalDialogueManager.instance.LoadDialogue("d" + "0" + (GetDay() + 1) + "_Dream");
    }


    public bool BuyCigarette()
    {
        if (PropertyManager.instance.GetMoney() < cigarettePrice) return false;

        PropertyManager.instance.UpdateMoney(-cigarettePrice);
        PropertyManager.instance.cigaretteCount += 1;
        return true;

    }

  

    public void WaitForFinishRepeatMission()
    {
        switch (GetDay())
        {
            case 0: // day 1
                Mission_Day[0].SetActive(true);
                break;
            case 1:// day 2
                Mission_Day[1].SetActive(true);
                break;
            case 2:// day 3
                Mission_Day[2].SetActive(true);
                break;
            case 3:// day 4
                Mission_Day[3].SetActive(true);
                break;
            case 4:// day 5
                Mission_Day[4].SetActive(true);
                break;
            case 5:// day 5
                Mission_Day[5].SetActive(true);
                break;
        }
    }

    public void OnCompleteRepeatMission()
    {
        SetCurrentGameMode(GameMode.Conversation);
        LocalDialogueManager.instance.LoadDialogue("d" + "0" + (GetDay() + 1)+ "_Morning_afterRepeatMission");
    }
}

//"Circle the words you want to revise, then click the button to request a revision. Some words are crucial and the author will refuse your revision request."
//"Circle the words you consider should be censored, then reject this piece. You will be penalized if you fail to select words that are supposed to be censored."
//"Make sure the piece has zero controversial words before you give it a pass. You can always ask for it to be revised. You will be penalized if you pass a piece that contains words that are supposed to be censored."