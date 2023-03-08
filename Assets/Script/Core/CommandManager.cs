using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CommandManager : MonoSingleton<CommandManager>
{
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
}
