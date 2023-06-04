using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaToolBook : MonoBehaviour
{
    // Start is called before the first frame update
    void Debug()
    {
        ViewManager.instance.UnloadTipView(); // unload all tip but don't destroy the object
        ViewManager.instance.LoadTipView(TipViewController.TipType.DialogueTip);

    }
}
