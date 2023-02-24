using System.Collections;
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

    void Start()
    {
        
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
