using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CommandManager : MonoSingleton<CommandManager>
{

    void Awake()
    {
        var objs = FindObjectsOfType<CommandManager>();

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

    [YarnCommand("StartWork")]
    public void TryStartWork()
    {
        GameManager.instance.StartWork();
    }

    [YarnCommand("GoToNextDay")]
    public void GoToNextWorkDay()
    {
        GameManager.instance.GoToNextWorkDay();
    }

    [YarnCommand("StartPaperShredder")]
    public void StartPaperShredder()
    {
        GameManager.instance.StartPaperShredder();
    }

    [YarnCommand("SetProperty")]
    public void SetProperty(string PropertyName, int change)
    {
        switch (PropertyName)
        {
            case "can_write_poem":
                //PropertyManager.instance.;
                break;
            case "can_listen_to_music":
                PropertyManager.instance.bCanListenToMusic = true;
                break;
            case "can_read_news":
                PropertyManager.instance.bCanReadNews = true;
                break;
            case "can_write":
                PropertyManager.instance.bCanWrite = true;
                break;
            case "hasCATgpt":
                PropertyManager.instance.hasCATgpt = true;
                break;
            case "bReflection":
                PropertyManager.instance.bReflection = true;
                break;
            case "bAesthetic":
                PropertyManager.instance.bAesthetic = true;
                break;
            case "bRebellious":
                PropertyManager.instance.bRebellious = true;
                break;
        }
       
    }

    [YarnCommand("UpdateProperty")]
    public void UpdateProperty(string PropertyName, int change)
    {
        switch (PropertyName)
        {
            case "can_write_poem":
                //PropertyManager.instance.;
                break;
        }
    }

    [YarnCommand("EndGame")]
    public void EndGame()
    {
        GameManager.instance.LoadEndGameScene();
    }

    [YarnFunction("CheckRebellious")]
    public static bool CheckRebellious()
    {
        return PropertyManager.instance.rebelliousCount > 3;
    }



}
