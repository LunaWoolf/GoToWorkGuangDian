using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoSingleton<GameManager>
{

    GameMode currentGameMode;


    public enum Character
    {
        BossHe,
        Li
    }

    public enum GameMode
    {
        Work,
        Conversation,
        Home
    }

    [Header("Word Day")]
    public DateTime gameDate = new DateTime(2019, 6, 6, 0, 0, 0);
    public int poemViewedToday = 0 ;
    public int MaxPoemNeedToViewToday = 5;



    public void SetCurrentGameMode(GameMode mode)
    {  currentGameMode = mode;

        switch (currentGameMode)
        {
            case GameMode.Work:
               StartWork();
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
        ViewManager.instance.UnloadAllView();
        ViewManager.instance.LoadWorkView();
        PoemGenerator.instance.TearPoem();
        PoemGenerator.instance.GeneratorPoem(5);
    }

    void Start()
    {
        Debug.Log(gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2"));
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void OnPoemPass()
    {
        TryGoToNextPoem();
    }

    public void OnPoemDeny()
    {
        TryGoToNextPoem();
    }

    void TryGoToNextPoem()
    {
        poemViewedToday++;
        if (poemViewedToday < MaxPoemNeedToViewToday)
        {
            PoemGenerator.instance.NextPoem();
        }
        else
        {
            EndOfWorkDay();
        }
    }

    void EndOfWorkDay()
    {
        PoemGenerator.instance.UnloadPoemPaper();
        LoadEndOfWorkDayDialogue();
        Debug.Log("End of day");
    }

    void LoadEndOfWorkDayDialogue()
    {
        string date = gameDate.Month.ToString("D2") + gameDate.Day.ToString("D2");
      
        LocalDialogueManager.instance.LoadDialogue("d" + date + "_EndWork");
        //0606_EndWork

    }
}
